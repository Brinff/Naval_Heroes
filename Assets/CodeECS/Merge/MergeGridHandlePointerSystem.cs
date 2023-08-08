using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(MergeGroup))]
[BurstCompile]
public partial struct MergeGridHandlePointerSystem : ISystemUpdate
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (mergePointer, pointerEvent, entity) in SystemAPI.Query<RefRW<MergeDragPointer>, RefRO<PointerDownEvent>>().WithDisabled<MergeBeginDragEvent>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            if (mergePointer.ValueRO.pointer == Entity.Null)
            {
                foreach (var (id, pointerEntity) in SystemAPI.Query<RefRO<PointerId>>().WithEntityAccess())
                {
                    if (PointerHelper.HasFlag(pointerEvent.ValueRO.value, id.ValueRO.value)) 
                    {
                        SystemAPI.SetComponentEnabled<MergeBeginDragEvent>(entity, true);
                        mergePointer.ValueRW.pointer = pointerEntity;
                        endEcb.SetComponentEnabled<MergeBeginDragEvent>(entity, false);
                        beginEcb.SetComponentEnabled<MergeUpdateDragEvent>(entity, true);  
                        break;
                    }
                }
            }
        }

        foreach (var (mergeGridPlacer, pointerEvent, entity) in SystemAPI.Query<RefRW<MergeDragPointer>, RefRO<PointerUpEvent>>().WithDisabled<MergeEndDragEvent>().WithAll<PointerHandlerTag>().WithEntityAccess())
        {
            if (mergeGridPlacer.ValueRW.pointer != Entity.Null && PointerHelper.HasFlag(pointerEvent.ValueRO.value, SystemAPI.GetComponentRO<PointerId>(mergeGridPlacer.ValueRO.pointer).ValueRO.value))
            {
                SystemAPI.SetComponentEnabled<MergeEndDragEvent>(entity, true);
                endEcb.SetComponent<MergeDragPointer>(entity, new MergeDragPointer());
                endEcb.SetComponentEnabled<MergeUpdateDragEvent>(entity, false);
                endEcb.SetComponentEnabled<MergeEndDragEvent>(entity, false);
            }
        }

    }
}
