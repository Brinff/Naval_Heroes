using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Game.Slots
{
    public class SlotGroup : MonoBehaviour
    {
        [SerializeField] private List<Slot> m_Slots = new List<Slot>();
        public IReadOnlyList<Slot> slots => m_Slots;

        public Slot GetSlotOverlap(Vector3 position, Rect rect)
        {
            List<(Slot slot, float weight)> slotDates = new List<(Slot slot, float weight)>();
            
            for (int i = 0; i < m_Slots.Count; i++)
            {
                var slot = m_Slots[i];
                if (slot.area.Overlap(position, rect, out float weight))
                {
                    slotDates.Add(new ValueTuple<Slot, float>(slot, weight));
                }
            }

            if (slotDates.Count > 0)
            {
                var firstArea = slotDates.OrderByDescending(x => x.weight).First();
                return firstArea.slot;
            }
            
            return null;
        }
    }
}