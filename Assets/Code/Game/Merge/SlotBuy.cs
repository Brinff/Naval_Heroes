using Game.Grid.Auhoring;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBuy : MonoBehaviour, ISlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public EntityData entity;

    public SlotItem item;

    public SlotCollection collection { get; private set; }


    private BoxCollider[] colliders;

    private void OnEnable()
    {
        Spawn();
    }

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (item == null)
        {
            item = slotItem;
            item.parentSlot = this;
            item.targetPosition = position;
            item.transform.position = position;
            return true;
        }
        return false;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        if (slotItem == item)
        {
            item = null;
            Spawn();
            return true;
        }
        return false;
    }

    [Button]
    public void Spawn()
    {
       AddItem(SlotItem.Create(entity, transform.position, transform.rotation), transform.position);
    }

    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;

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
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        item.As<IBeginDragHandler>()?.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item.As<IEndDragHandler>()?.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        item.As<IDragHandler>()?.OnDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var mergeSlots = collection.GetSlots<SlotMerge>();
        foreach (var slot in mergeSlots)
        {
            if (slot.AddItem(item, slot.transform.position))
            {
                RemoveItem(item);
                break;
            }
        }
    }
}
