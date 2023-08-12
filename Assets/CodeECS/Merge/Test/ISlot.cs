using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Merge.Engine
{
    public interface ISlot
    {
        public bool Populate(SlotItem item, Vector3 position, out Vector3 targetPosition);
    }
}
