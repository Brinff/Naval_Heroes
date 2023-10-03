using Game.Grid.Auhoring;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Game.Merge.Data;
using Game.Utility;
using DG.Tweening;

public class SlotMerge : MonoBehaviour, ISlotPopulate, IBeginDragHandler, IDragHandler, IEndDragHandler, ISlotRenderer, IItemBeginDrag, IItemEndDrag
{
    public MergeDatabase data;


    [SerializeField]
    private float m_ExpandBorderRaycast = 0.5f;

    [SerializeField]
    private Color m_ColorDefault;
    [SerializeField]
    private Color m_ColorAccept;
    [SerializeField]
    private Color m_ColorReject;
    [SerializeField]
    private float m_HighlightDefault = 1;
    [SerializeField]
    private float m_HighlightActive = 2;

    public SlotCollection collection { get; private set; }

    public List<SlotItem> items { get; private set; } = new List<SlotItem>();

    public SlotItem item => items.Count > 0 ? items[0] : null;
    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;

    private EntityData result;

    [SerializeField]
    private SpriteRenderer m_GridRenderer;
    private MaterialPropertyBlock m_MaterialPropertyBlock;
    public bool AddItem(SlotItem item, Vector3 position)
    {
        if (items.Count == 0)
        {
            items.Add(item);
            collection.SetDirty();
            item.parentSlot = this;
            item.targetPosition = transform.position;
            item.transform.position = transform.position;

            return true;
        }
        else if (result)
        {
            var firstItem = items[0];

            RemoveItem(firstItem);

            Destroy(firstItem.info.gameObject);
            Destroy(firstItem.entity);
            Destroy(firstItem.gameObject);

            items.Add(item);

            item.parentSlot = this;
            item.targetPosition = transform.position;
            item.transform.position = transform.position;
            item.SetEntity(result);
            collection.SetDirty();

            result = null;

            return true;
        }
        return false;
    }

    public bool RemoveItem(SlotItem item)
    {
        if (items.Remove(item))
        {
            collection.SetDirty();
            return true;
        }
        return false;
    }

    public bool Populate(Ray ray, SlotItem item, out Vector3 position)
    {
        Plane plane = new Plane(Vector3.up, 0);
        position = Vector3.zero;

        SetColor(m_ColorDefault, m_HighlightActive);

        if (plane.Raycast(ray, out float distance))
        {
            result = null;
            position = ray.GetPoint(distance);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].Raycast(ray, out RaycastHit _, 10000))
                {
                    SetColor(m_ColorAccept, m_HighlightActive);

                    if (this.item == null) return true;



                    result = data.GetResult(this.item.entityData, item.entityData);

                    
                    if (result != null) return true;
                    SetColor(m_ColorReject, m_HighlightActive);
                }
            }
        }
        return false;
    }


    private void SetColor(Color color, float highlight)
    {
        if (m_MaterialPropertyBlock == null) m_MaterialPropertyBlock = new MaterialPropertyBlock();
        m_GridRenderer.GetPropertyBlock(m_MaterialPropertyBlock);
        m_MaterialPropertyBlock.SetColor("_Color", color * highlight);
        m_GridRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }

    public void Prepare(SlotCollection collection)
    {

        GridAuhoring grid = gameObject.GetComponent<GridAuhoring>();

        SetColor(m_ColorDefault, m_HighlightDefault);

        //var gridRenderer = GetComponent<GridRendererAuthoring>();
        //gridRenderer.BeginFill(grid.scale, grid.center);


        colliders = new BoxCollider[grid.rects.Length];
        for (int i = 0; i < grid.rects.Length; i++)
        {
            var sigleRect = grid.GetSingleRect(grid.rects[i]);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(sigleRect.center.x, 0, sigleRect.center.y);
            boxCollider.size = new Vector3(sigleRect.size.x + m_ExpandBorderRaycast, 1, sigleRect.size.y + m_ExpandBorderRaycast);
            colliders[i] = boxCollider;
            //gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        }

        //gridRenderer.EndFill();



        this.collection = collection;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        item.As<IBeginDragHandler>()?.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        item.As<IDragHandler>()?.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item.As<IEndDragHandler>()?.OnEndDrag(eventData);
    }

    public void Show(float duration)
    {
        item?.Show();
        m_GridRenderer.DOFade(1, duration);
    }

    public void Hide(float duration)
    {
        item?.Hide();
        m_GridRenderer.DOFade(0, duration);
    }

    public bool AddItemPossible(SlotItem slotItem, Vector3 position)
    {
        if (items.Count == 0) return true;
        return false;
    }

    public void ItemBeginDrag(SlotItem slotItem)
    {
        SetColor(m_ColorDefault, m_HighlightActive);
    }

    public void ItemEndDrag(SlotItem slotItem)
    {
        SetColor(m_ColorDefault, m_HighlightDefault);
    }

    public bool RemoveItemPossible(SlotItem slotItem, Vector3 position)
    {
        return true;
    }
}
