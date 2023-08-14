
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[BurstCompile]
public partial struct BoundsSystem : ISystem
{
    [BurstCompile]
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
            var min = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.min);
            var max = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.max);
            var center = math.lerp(min, max, 0.5f);

            worldBounds.ValueRW.min = math.min(min, center);
            worldBounds.ValueRW.max = math.max(max, center);




            //worldBounds.ValueRW.max = TransformHelpers.TransformPoint(localTransform.ValueRO.Value, localBounds.ValueRO.max);
        }
    }

    public void Encapsulte()
    {

    }


}
