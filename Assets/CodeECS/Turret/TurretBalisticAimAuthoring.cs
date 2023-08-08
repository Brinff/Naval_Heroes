using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public struct TurretBalisticAim : IComponentData
{
    public Entity origin;
    public float velocity;
}

public class TurretBalisticAimAuthoring : MonoBehaviour
{
    public Transform origin;
    public float velocity;
    public RotationConstrainAuthoring[] constrains;
}

public class TurretBalisticAimBaker : Baker<TurretBalisticAimAuthoring>
{
    public override void Bake(TurretBalisticAimAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent<TurretBalisticAim>(entity, new TurretBalisticAim() { origin = GetEntity(authoring.transform, TransformUsageFlags.Dynamic), velocity = authoring.velocity });
        if (authoring.constrains != null && authoring.constrains.Length > 0)
        {
            var buffer = AddBuffer<TurretConstrains>(entity);
            foreach (var constrain in authoring.constrains)
            {
                if (constrain != null) buffer.Add(new TurretConstrains() { value = GetEntity(constrain, TransformUsageFlags.Dynamic) });
            }
        }
    }
}
