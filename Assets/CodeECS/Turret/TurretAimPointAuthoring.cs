using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TurretAimPointAuthoring : MonoBehaviour
{
    public Vector3 point;
}

public class TurretAimPointBaker : Baker<TurretAimPointAuthoring>
{
    public override void Bake(TurretAimPointAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new TurretAimPoint() { value = authoring.point });
    }
}
