using Game.Pointer.Data;
using JetBrains.Annotations;
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerClickEventSystem))]
[BurstCompile]
public partial struct PointerDropEventSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (dragEntity, pointerDropEntity, fristHoveredEntity, id, entity) in SystemAPI.Query<RefRO<PointerDragEntity>, RefRW<PointerDropEntity>, RefRO<PointerFirstHoveredEntity>, RefRO<PointerId>>().WithAll<PointerUpEvent>().WithEntityAccess())
        {
            if (dragEntity.ValueRO.value != Entity.Null)
            {
                if (fristHoveredEntity.ValueRO.value != null && fristHoveredEntity.ValueRO.value != dragEntity.ValueRO.value)
                {
                    if (SystemAPI.HasComponent<PointerDropEvent>(fristHoveredEntity.ValueRO.value))
                    {
                        RefRW<PointerDropEvent> pointerDropEvent = SystemAPI.GetComponentRW<PointerDropEvent>(fristHoveredEntity.ValueRO.value);
                        if (!PointerHelper.HasFlag(pointerDropEvent.ValueRW.value, id.ValueRO.value)) pointerDropEvent.ValueRW.value |= id.ValueRO.value;
                        pointerDropEntity.ValueRW.value = fristHoveredEntity.ValueRO.value;
                        beginEcb.SetComponentEnabled<PointerDropEvent>(fristHoveredEntity.ValueRO.value, true);
                    }

                    if (SystemAPI.HasComponent<PointerDropEvent>(fristHoveredEntity.ValueRO.value))
                    {
                        RefRW<PointerDropEvent> pointerDropEvent = SystemAPI.GetComponentRW<PointerDropEvent>(fristHoveredEntity.ValueRO.value);
                        if (!PointerHelper.HasFlag(pointerDropEvent.ValueRW.value, id.ValueRO.value)) pointerDropEvent.ValueRW.value |= id.ValueRO.value;
                        pointerDropEntity.ValueRW.value = fristHoveredEntity.ValueRO.value;
                        beginEcb.SetComponentEnabled<PointerDropEvent>(fristHoveredEntity.ValueRO.value, true);
                    }
                }
            }
        }

        foreach (var pointerDropEvent in SystemAPI.Query<RefRO<PointerDropEvent>>())
        {
            foreach (var (pointerId, entity) in SystemAPI.Query<RefRO<PointerId>>().WithAll<PointerDropEntity>().WithEntityAccess())
            {
                if (PointerHelper.HasFlag(pointerDropEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    endEcb.SetComponent(entity, new PointerDropEntity());
                }
            }
        }
    }
}
