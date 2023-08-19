using Game.Merge.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SlotCollection : MonoBehaviour
{
    private List<ISlot> slots = new List<ISlot>();

    public void RegisterSlot(ISlot slot)
    {
        if (!slots.Contains(slot))
        {
            slots.Add(slot);
            slot.Prepare(this);
        }
    }

    private void OnEnable()
    {
        var slots = GetComponentsInChildren<ISlot>();
        foreach (var item in slots)
        {
            RegisterSlot(item);
        }
    }

    public T[] GetSlots<T>() where T : ISlot
    {
        return slots.Where(x => x is T).Select(x => (T)x).ToArray<T>();
    }



}
