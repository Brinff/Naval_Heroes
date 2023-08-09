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
        var beginECB = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (localTransform, localBounds, entity) in SystemAPI.Query<RefRO<LocalToWorld>, RefRO<LocalBounds>>().WithNone<WorldBounds>().WithEntityAccess())
        {
            WorldBounds worldBounds = new WorldBounds() { min = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.min), max = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.max) };
            beginECB.AddComponent(entity, worldBounds);
        }

        foreach (var (localTransform, localBounds, worldBounds) in SystemAPI.Query<RefRO<LocalToWorld>, RefRO<LocalBounds>, RefRW<WorldBounds>>())
        {
            worldBounds.ValueRW.min = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.min);
            worldBounds.ValueRW.max = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.max);
        }
    }
}
