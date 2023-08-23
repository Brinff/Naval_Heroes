using Game.Grid.Auhoring;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Game.Merge.Data;
using Game.Utility;

public class SlotMerge : MonoBehaviour, ISlotPopulate, IBeginDragHandler, IDragHandler, IEndDragHandler, ISlotRenderer
{
    public MergeDatabase data;
    public SlotCollection collection { get; private set; }

    public List<SlotItem> items { get; private set; } = new List<SlotItem>();

    public SlotItem item => items.Count > 0 ? items[0] : null;
    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;

    private EntityData result;

    [SerializeField]
    private GridRendererAuthoring m_GridRenderer;

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
        if (plane.Raycast(ray, out float distance))
        {
            result = null;
            position = ray.GetPoint(distance);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].Raycast(ray, out RaycastHit _, 10000))
                {
                    if (this.item == null) return true;

                    result = data.GetResult(this.item.entityData, item.entityData);

                    if (result != null) return true;
                }
            }
        }
        return false;
    }

    public void Prepare(SlotCollection collection)
    {
        GridAuhoring grid = gameObject.GetComponent<GridAuhoring>();

        var gridRenderer = GetComponent<GridRendererAuthoring>();
        gridRenderer.BeginFill(grid.scale, grid.center);


        colliders = new BoxCollider[grid.rects.Length];
        for (int i = 0; i < grid.rects.Length; i++)
        {
            var sigleRect = grid.GetSingleRect(grid.rects[i]);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(sigleRect.center.x, 0, sigleRect.center.y);
            boxCollider.size = new Vector3(sigleRect.size.x, 1, sigleRect.size.y);
            colliders[i] = boxCollider;
            gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        }

        gridRenderer.EndFill();



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
        m_GridRenderer.DoAlpha(1, duration);
    }

    public void Hide(float duration)
    {
        item?.Hide();
        m_GridRenderer.DoAlpha(0, duration);
    }
}
