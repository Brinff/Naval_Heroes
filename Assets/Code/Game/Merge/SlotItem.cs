using Game.Grid.Auhoring;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EntityData entityData;
    public GameObject entity;
    public GridAuhoring grid;
    [HideInInspector]
    public SlotItemInfoItem info;

    public ISlot parentSlot;
    public ISlot populateSlot;

    private BoxCollider[] boxColliders;

    private float height;


    public bool isLockDrag { get; set; }

    public void Show()
    {
        gameObject.SetActive(true);
        entity.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        entity.SetActive(false);
    }

    private bool isDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLockDrag) return;
        if (parentSlot!=null)
        {
            if(parentSlot.RemoveItemPossible(this, Vector3.zero))
            {
                var otherSlots = parentSlot.collection.GetSlots<IItemBeginDrag>();
                foreach (var item in otherSlots)
                {
                    item.ItemBeginDrag(this);
                }

                isDrag = true;
                height = GameSettings.Instance.merge.moveItem.height;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        var otherSlots = parentSlot.collection.GetSlots<ISlotPopulate>();
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (plane.Raycast(ray, out float distance))
        {
            var point = ray.GetPoint(distance);
            populateSlot = null;
            foreach (var otherSlot in otherSlots)
            {
                if (otherSlot.Populate(ray, this, out Vector3 newPosition))
                {
                    populateSlot = otherSlot;
                    point = newPosition;
                }
            }
            targetRotation = Quaternion.identity;
            targetScale = 1;
            targetPosition = point;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        if (populateSlot == null)
        {
            targetPosition = position;
            targetRotation = rotation;
            targetScale = scale;
        }
        else
        {
            if (parentSlot.RemoveItem(this))
            {
                populateSlot.AddItem(this, targetPosition);
            }
            else
            {
                targetPosition = position;
                targetRotation = rotation;
                targetScale = scale;
            }
        }

        var otherSlots = parentSlot.collection.GetSlots<IItemEndDrag>();
        foreach (var item in otherSlots)
        {
            item.ItemEndDrag(this);
        }

        height = 0;

        isDrag = false;
    }



    public void SetEntity(EntityData entityData)
    {
        if (boxColliders != null)
        {
            for (int i = 0; i < boxColliders.Length; i++)
            {
                Destroy(boxColliders[i]);
            }
        }

        if (entity != null)
        {
            Destroy(entity);
        }

        this.entityData = entityData;
        entity = Instantiate(entityData.prefab, m_CurrentPosition, m_CurrentRotation);

        gameObject.name = $"Slot Item {entityData.name}";

        grid = entity.GetComponent<GridAuhoring>();

        boxColliders = new BoxCollider[grid.rects.Length];
        for (int i = 0; i < grid.rects.Length; i++)
        {
            var rect = grid.rects[i];
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            var singleRect = grid.GetSingleRect(rect);
            boxCollider.center = new Vector3(singleRect.center.x, 0, singleRect.center.y);
            boxCollider.size = new Vector3(singleRect.size.x, 1, singleRect.size.y);
            boxColliders[i] = boxCollider;
        }

        info.SetRare(entity.GetComponent<RareAuthoring>().rareData.color);
        info.SetClassification(entity.GetComponent<ClassificationAuhtoring>().classData.icon);
        info.SetLevel(entity.GetComponent<GradeLevelAuthoring>().amount);
        info.UpdatePosition(entity.GetComponent<InfoAuthoring>().orgin.position);
    }

    //[SerializeField]
    //public float m_Damper;
    //[SerializeField]
    //public float m_Force;
    private Vector3 m_CurrentPosition;
    private Quaternion m_CurrentRotation = Quaternion.identity;
    private float m_CurrentScale = 1;

    private Vector3 m_TargetPosition;
    private Quaternion m_TargetRotation = Quaternion.identity;
    private float m_TargetScale = 1;

    private Vector3 m_Velocity;
    //[SerializeField]
    //private Vector3 m_RotationByLocalVelocty;
    //[SerializeField]
    //private float m_ClampRotationByVelocity;

    private void Update()
    {
        var gameSettings = GameSettings.Instance;
        var target = m_TargetPosition + Vector3.up * height;
        //var direction = Vector3.Normalize(target - m_CurrentPosition);
        //var distance = Vector3.Distance(m_CurrentPosition, target);




        //m_Velocity += direction * gameSettings.merge.moveItem.force * distance * Time.deltaTime;
        //m_Velocity -= m_Velocity * gameSettings.merge.moveItem.damper * Time.deltaTime;


        //Vector3 localVelocity = transform.InverseTransformVector(m_Velocity);
        //localVelocity = Vector3.Scale(localVelocity, gameSettings.merge.moveItem.rotationByLocalVelocty);
        //Vector3 velocity = transform.TransformVector(localVelocity);

        //if (velocity != Vector3.zero)
        //{
        //    Vector3 right = Vector3.Cross(velocity, Vector3.up);
        //    float angle = Mathf.Clamp(velocity.magnitude, 0, gameSettings.merge.moveItem.clampRotationByVelocity);
        //    m_CurrentRotation = Quaternion.AngleAxis(angle, right);
        //}
        //m_CurrentPosition += m_Velocity * Time.deltaTime;

        m_CurrentPosition = Vector3.Lerp(m_CurrentPosition, target, Time.deltaTime * 20);
        m_CurrentRotation = Quaternion.Lerp(m_CurrentRotation, m_TargetRotation, Time.deltaTime * 20);
        m_CurrentScale = Mathf.Lerp(m_CurrentScale, m_TargetScale, Time.deltaTime * 20);
        if (entity)
        {
            entity.transform.rotation = m_CurrentRotation;
            entity.transform.position = m_CurrentPosition;
            entity.transform.localScale = Vector3.one * m_CurrentScale;
        }

        if (info)
        {
            info.UpdatePosition(entity.GetComponent<InfoAuthoring>().orgin.position);
        }
    }

    public Vector3 currentPosition { get => m_CurrentPosition; set { m_TargetPosition = value; m_Velocity = Vector3.zero; m_CurrentPosition = value; } }
    public Vector3 targetPosition { get => m_TargetPosition; set => m_TargetPosition = value; }
    public Quaternion targetRotation { get => m_TargetRotation; set => m_TargetRotation = value; }
    public float targetScale { get => m_TargetScale; set => m_TargetScale = value; }

    private Quaternion m_Rotation;
    private float m_Scale;

    public Vector3 position { get => transform.position; set => transform.position = value; }
    public float scale { get => m_Scale; set => m_Scale = value; }
    public Quaternion rotation { get => m_Rotation; set => m_Rotation = value; }

    public static SlotItem Create(SlotCollection collection, EntityData entityData, Vector3 position, Quaternion rotation, float scale)
    {
        GameObject gameObject = new GameObject($"Slot Item {entityData.name}");
        gameObject.layer = 9;


        var slotItem = gameObject.AddComponent<SlotItem>();
        var widget = UISystem.Instance.GetElement<SlotItemInfoWidget>();

        slotItem.info = widget.Create();
        slotItem.transform.SetParent(collection.transform);
        slotItem.currentPosition = position;

        slotItem.targetPosition = position;
        slotItem.targetRotation = rotation;
        slotItem.targetScale = scale;

        slotItem.position = position;
        slotItem.rotation = rotation;
        slotItem.scale = scale;

        slotItem.SetEntity(entityData);

        

        return slotItem;
    }
}
