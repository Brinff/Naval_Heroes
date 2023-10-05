
using System.Collections.Generic;
using UnityEngine;

public interface ISlot
{
    public int id { get; }
    public List<SlotItem> items { get; }
    public SlotCollection collection { get; }
    public void Prepare(SlotCollection collection);
    public bool RemoveItemPossible(SlotItem slotItem, Vector3 position);
    public bool AddItemPossible(SlotItem slotItem, Vector3 position);
    public bool AddItem(SlotItem slotItem, Vector3 position);
    public bool RemoveItem(SlotItem slotItem);
}

public interface ISlotRenderer : ISlot
{
    public void Show(float duration);
    public void Hide(float duration);
}

public interface ISlotPopulate : ISlot
{
    public bool Populate(Ray ray, SlotItem slotItem, out Vector3 position);
}

public interface IItemBeginDrag : ISlot
{
    public void ItemBeginDrag(SlotItem slotItem);
}

public interface IItemEndDrag : ISlot
{
    public void ItemEndDrag(SlotItem slotItem);
}

