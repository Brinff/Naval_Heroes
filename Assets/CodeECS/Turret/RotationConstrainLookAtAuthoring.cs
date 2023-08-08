using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public struct RotationConstrainLookAtPointComponent : IComponentData
{
    public float3 value;
}

public struct RotationConstrainLookAtEntityComponent : IComponentData
{
    public Entity value;
}

public class RotationConstrainLookAtAuthoring : MonoBehaviour
{
    public bool isUpdate = false;
    public Transform target;
}

public class RotationConstrainLookAtBaker : Baker<RotationConstrainLookAtAuthoring>
{
    public override void Bake(RotationConstrainLookAtAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new RotationConstrainLookAtPointComponent() { value = authoring.target.position });
        if(authoring.isUpdate) AddComponent(entity, new RotationConstrainLookAtEntityComponent() { value = GetEntity(authoring.target.gameObject, TransformUsageFlags.Dynamic) });
    }
}
