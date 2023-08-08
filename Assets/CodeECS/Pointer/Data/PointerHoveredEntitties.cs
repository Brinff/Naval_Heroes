using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PointerFirstHoveredEntity : IComponentData
{
    public Entity value;
}

public struct PointerHoveredEntity : IBufferElementData
{
    public float distance;
    public Entity entity;
}
