using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct WorldTransformAspect : IAspect
{
    readonly Entity entity;
    readonly RefRO<LocalToWorld> localToWorld;
    readonly RefRW<LocalTransform> localTransform;
    public float3 position { get => localToWorld.ValueRO.Position; }
    public quaternion rotation { get => localToWorld.ValueRO.Rotation; }
    public float scale => localTransform.ValueRO.Scale;
}
