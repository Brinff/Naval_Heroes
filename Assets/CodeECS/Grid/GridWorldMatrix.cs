using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct GridWorldToLocal : IComponentData
{
    public float4x4 value;
}

public struct GridLocalToWorld : IComponentData
{
    public float4x4 value;
}