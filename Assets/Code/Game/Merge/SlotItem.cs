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
            targetPosition = point;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDrag) return;

        if (populateSlot == null)
        {
            targetPosition = transform.position;
        }
        else
        {
            if (parentSlot.RemoveItem(this))
            {
                populateSlot.AddItem(this, targetPosition);
            }
            else targetPosition = transform.position;
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
    private Vector3 m_TargetPosition;
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

        if (entity)
        {
            entity.transform.rotation = m_CurrentRotation;
            entity.transform.position = m_CurrentPosition;
        }

        if (info)
        {
            info.UpdatePosition(entity.GetComponent<InfoAuthoring>().orgin.position);
        }
    }

    public Vector3 currentPosition { get => m_CurrentPosition; set { m_TargetPosition = value; m_Velocity = Vector3.zero; m_CurrentPosition = value; } }
    public Vector3 targetPosition { get => m_TargetPosition; set => m_TargetPosition = value; }
    public Vector3 position { get => transform.position; set => transform.position = value; }

    public static SlotItem Create(SlotCollection collection, EntityData entityData, Vector3 position)
    {
        GameObject gameObject = new GameObject($"Slot Item {entityData.name}");
        gameObject.layer = 9;


        var slotItem = gameObject.AddComponent<SlotItem>();
        var widget = UISystem.Instance.GetElement<SlotItemInfoWidget>();

        slotItem.info = widget.Create();
        slotItem.transform.SetParent(collection.transform);
        slotItem.currentPosition = position;
        slotItem.targetPosition = position;
        slotItem.position = position;
        slotItem.SetEntity(entityData);

        

        return slotItem;
    }
}
