using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct BoundsSystem : ISystemUpdate
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, worldBounds, localBounds) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<WorldBounds>, RefRO<LocalBounds>>())
        {
            worldBounds.ValueRW.min = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.min);
            worldBounds.ValueRW.max = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.max);
        }
    }
}
