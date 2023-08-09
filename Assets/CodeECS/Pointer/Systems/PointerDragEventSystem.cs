using Game.Pointer.Data;
using Game.Pointer.Events;

using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerDropEventSystem))]
[BurstCompile]
public partial struct PointerDragEventSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (dragEdntity, id, firstHoveredEntity) in SystemAPI.Query<RefRW<PointerDragEntity>, RefRO<PointerId>, RefRO<PointerFirstHoveredEntity>>().WithAll<PointerDownEvent>())
        {
            if (SystemAPI.HasComponent<PointerBeginDragEvent>(firstHoveredEntity.ValueRO.value))
            {
                dragEdntity.ValueRW.value = firstHoveredEntity.ValueRO.value;

                RefRW<PointerBeginDragEvent> pointerBeginDragEvent = SystemAPI.GetComponentRW<PointerBeginDragEvent>(dragEdntity.ValueRO.value);
                if (!PointerHelper.HasFlag(pointerBeginDragEvent.ValueRW.value, id.ValueRO.value)) pointerBeginDragEvent.ValueRW.value |= id.ValueRO.value;
                beginEcb.SetComponentEnabled<PointerBeginDragEvent>(dragEdntity.ValueRO.value, true);
            }

            if (SystemAPI.HasComponent<PointerUpdateDragEvent>(firstHoveredEntity.ValueRO.value))
            {
                dragEdntity.ValueRW.value = firstHoveredEntity.ValueRO.value;

                RefRW<PointerUpdateDragEvent> pointerBeginDragEvent = SystemAPI.GetComponentRW<PointerUpdateDragEvent>(dragEdntity.ValueRO.value);
                if (!PointerHelper.HasFlag(pointerBeginDragEvent.ValueRW.value, id.ValueRO.value)) pointerBeginDragEvent.ValueRW.value |= id.ValueRO.value;
                beginEcb.SetComponentEnabled<PointerUpdateDragEvent>(dragEdntity.ValueRO.value, true);
            }
        }

        foreach (var (dragEntity, id, entity) in SystemAPI.Query<RefRO<PointerDragEntity>, RefRO<PointerId>>().WithAll<PointerUpEvent>().WithEntityAccess())
        {
            if (dragEntity.ValueRO.value != Entity.Null)
            {
                if (SystemAPI.HasComponent<PointerUpdateDragEvent>(dragEntity.ValueRO.value))
                {
                    RefRW<PointerUpdateDragEvent> pointerEndDragEvent = SystemAPI.GetComponentRW<PointerUpdateDragEvent>(dragEntity.ValueRO.value);
                    if (!PointerHelper.HasFlag(pointerEndDragEvent.ValueRW.value, id.ValueRO.value)) pointerEndDragEvent.ValueRW.value |= id.ValueRO.value;
                    beginEcb.SetComponentEnabled<PointerUpdateDragEvent>(dragEntity.ValueRO.value, false);
                }

                if (SystemAPI.HasComponent<PointerEndDragEvent>(dragEntity.ValueRO.value))
                {
                    RefRW<PointerEndDragEvent> pointerEndDragEvent = SystemAPI.GetComponentRW<PointerEndDragEvent>(dragEntity.ValueRO.value);
                    if (!PointerHelper.HasFlag(pointerEndDragEvent.ValueRW.value, id.ValueRO.value)) pointerEndDragEvent.ValueRW.value |= id.ValueRO.value;
                    beginEcb.SetComponentEnabled<PointerEndDragEvent>(dragEntity.ValueRO.value, true);
                }
            }
        }

        foreach (var pointerEndDragEvent in SystemAPI.Query<RefRO<PointerEndDragEvent>>())
        {
            foreach (var (pointerId, entity) in SystemAPI.Query<RefRO<PointerId>>().WithAll<PointerDragEntity>().WithEntityAccess())
            {
                if (PointerHelper.HasFlag(pointerEndDragEvent.ValueRO.value, pointerId.ValueRO.value))
                {
                    endEcb.SetComponent(entity, new PointerDragEntity());
                }
            }
        }
    }
}
