
using UnityEngine;

public interface ISlot
{
    public SlotCollection collection { get; }
    public void Prepare(SlotCollection collection);
    public bool AddItem(SlotItem slotItem, Vector3 position);
    public bool RemoveItem(SlotItem slotItem);
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

