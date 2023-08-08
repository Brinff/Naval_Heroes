using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PointerDownEvent : IComponentData, IEnableableComponent
{
    public int value;
}
