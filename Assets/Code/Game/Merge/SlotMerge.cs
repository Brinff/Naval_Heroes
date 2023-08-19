using Game.Grid.Auhoring;
using Game.Merge.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.AI;

public class SlotMerge : MonoBehaviour, ISlotPopulate, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MergeData data;
    public SlotCollection collection { get; private set; }

    public SlotItem item;

    public BoxCollider[] colliders;

    public EntityData result;

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (item == null)
        {
            item = slotItem;
            item.parentSlot = this;
            item.targetPosition = transform.position;
            item.transform.position = transform.position;

            return true;
        }
        else if (result)
        {
            Destroy(item.entity);
            Destroy(item.gameObject);
            item = slotItem;
            item.parentSlot = this;
            item.targetPosition = transform.position;
            item.transform.position = transform.position;
            item.SetEntity(result);
            result = null;

            return true;
        }
        return false;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        if (slotItem == item)
        {
            item = null;
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
}
