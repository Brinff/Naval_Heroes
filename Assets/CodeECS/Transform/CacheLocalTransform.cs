using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Android;

public struct CacheLocalTransform : IComponentData
{
    public float3 position;
    public Quaternion rotation;
    public float scale;


    public static CacheLocalTransform Create(LocalTransform localTransform)
    {
        return new CacheLocalTransform() { position = localTransform.Position, rotation = localTransform.Rotation, scale = localTransform.Scale };
    }

    public static void Apply(RefRW<LocalTransform> localTransform, CacheLocalTransform cacheLocalTransform)
    {
        localTransform.ValueRW.Position = cacheLocalTransform.position;
        localTransform.ValueRW.Rotation = cacheLocalTransform.rotation;
        localTransform.ValueRW.Scale = cacheLocalTransform.scale;
    }
}
