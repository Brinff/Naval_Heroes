using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

public struct WorldBounds : IComponentData
{
    public float3 min;
    public float3 max;
}

public struct LocalBounds : IComponentData
{
    public float3 min;
    public float3 max;
}