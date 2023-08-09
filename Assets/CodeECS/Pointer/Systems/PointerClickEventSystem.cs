
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerPressEventSystem))]
[BurstCompile]
public partial struct PointerClickEventSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (pressEntity, id, firstHoveredEntity) in SystemAPI.Query<RefRO<PointerPressEntity>, RefRO<PointerId>, RefRO<PointerFirstHoveredEntity>>().WithAll<PointerUpEvent>())
        {
            if (pressEntity.ValueRO.entity == firstHoveredEntity.ValueRO.value)
            {
                if (SystemAPI.HasComponent<PointerClickEvent>(pressEntity.ValueRO.entity))
                {
                    RefRW<PointerClickEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerClickEvent>(pressEntity.ValueRO.entity);
                    if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, id.ValueRO.value)) pointerClickEvent.ValueRW.value |= id.ValueRO.value;
                    beginEcb.SetComponentEnabled<PointerClickEvent>(pressEntity.ValueRO.entity, true);
                }
            }
        }
    }
}
