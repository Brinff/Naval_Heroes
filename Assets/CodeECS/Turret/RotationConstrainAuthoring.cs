using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct RotationConstrainTargetComponent : IComponentData
{
    public quaternion rotation;
}

public struct RotationConstrainAxisComponent : IComponentData
{
    public float3 axis;
}

public struct RotationConstrainClampComponent : IComponentData
{
    public float minAngle;
    public float maxAngle;
}

public struct RotationConstrainSmoothComponent : IComponentData
{
    public float time;
}

public class RotationConstrainAuthoring : MonoBehaviour
{
    public Quaternion rotation;
    public Vector3 axis;
    public bool isClamp;
    public float minAngle;
    public float maxAngle;
    public float smoothTime;
}

public class RotationConstrainBaker : Baker<RotationConstrainAuthoring>
{
    public override void Bake(RotationConstrainAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new RotationConstrainTargetComponent() { rotation = authoring.rotation });
        AddComponent(entity, new RotationConstrainAxisComponent() { axis = authoring.axis });
        if (authoring.isClamp) AddComponent(entity, new RotationConstrainClampComponent() { minAngle = authoring.minAngle, maxAngle = authoring.maxAngle });
        if (authoring.smoothTime > 0) AddComponent(entity, new RotationConstrainSmoothComponent() { time = authoring.smoothTime });
    }
}