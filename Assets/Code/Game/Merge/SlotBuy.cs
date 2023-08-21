using Game.Grid.Auhoring;
using Game.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBuy : MonoBehaviour, ISlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, ISlotRenderer
{
    public EntityData entity;

    public List<SlotItem> items { get; private set; } = new List<SlotItem>();
    public SlotItem item => items.Count > 0 ? items[0] : null;
    public SlotCollection collection { get; private set; }

    [SerializeField]
    private GridRendererAuthoring gridRenderer;

    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (items.Count == 0)
        {
            items.Add(slotItem);
            slotItem.parentSlot = this;
            slotItem.targetPosition = position;
            slotItem.transform.position = position;
            return true;
        }
        return false;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        if (items.Remove(slotItem))
        {
            Spawn();
            return true;
        }
        return false;
    }

    [Button]
    public void Spawn()
    {
        AddItem(SlotItem.Create(collection, entity, transform.position), transform.position);
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
            boxCollider.size = new Vector3(sigleRect.size.x, 5, sigleRect.size.y);
            colliders[i] = boxCollider;
            gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        }

        gridRenderer.EndFill();

        Spawn();
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

    public void Show(float duration)
    {
        item.Show();
        gridRenderer.DoAlpha(1, duration);
    }

    public void Hide(float duration)
    {
        item.Hide();
        gridRenderer.DoAlpha(0, duration);
    }
}
