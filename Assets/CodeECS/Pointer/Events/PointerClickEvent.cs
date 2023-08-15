using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game.Pointer.Events
{
    public struct PointerClickEvent : IComponentData, IEnableableComponent
    {
        public int value;
    }
}
