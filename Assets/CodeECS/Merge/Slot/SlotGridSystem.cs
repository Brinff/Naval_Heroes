using Unity.Burst;
using Unity.Entities;

using Game.Merge.Components;
using Game.Debug;
using Game.Merge.Groups;
using Game.Pointer.Events;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SlotGroup)), UpdateAfter(typeof(SlotMergeSystem))]
    public partial struct SlotGridSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            //foreach (var (slotGrid, slotGridEnterEvent, entity) in SystemAPI.Query<SlotGrid, RefRO<PointerEnterEvent>>().WithEntityAccess())
            //{
            //    Debug.DebugSystem.Log("Enter Grid");
            //}

            //foreach (var (slotGrid, slotGridExitEvent, entity) in SystemAPI.Query<SlotGrid, RefRO<PointerExitEvent>>().WithEntityAccess())
            //{
            //    Debug.DebugSystem.Log("Exit Grid");
            //}


            //foreach (var (pointerId, pointerRay) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>>())
            //{
            //    foreach (var (slotEntityPosition, entity) in SystemAPI.Query<RefRW<SlotEntityPosition>>().WithOptions(EntityQueryOptions.FilterWriteGroup).WithEntityAccess())
            //    {
            //        if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
            //        {
            //            if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, localToWorld.ValueRO.Position, localToWorld.ValueRO.Up, out float distance))
            //            {
            //                slotEntityPosition.ValueRW.value = math.round(geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance) / 30) * 30;
            //            }
            //            break;
            //        }
            //    }
            //}



            foreach (var (slotGrid, slotEntity, slotGridTransform, slotPosition, slotDropEvent, entity) in SystemAPI.Query<SlotGrid, RefRW<SlotEntity>, GridTransformAspect, RefRO<SlotPosition>, RefRO<PointerDropEvent>>().WithEntityAccess())
            {
                foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
                {
                    if (PointerHelper.HasFlag(slotDropEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        if (SystemAPI.HasComponent<SlotEntity>(pointerDragEntity.ValueRO.value))
                        {
                            DebugSystem.Log("Drop");
                            var dragSlotEntity = SystemAPI.GetComponentRW<SlotEntity>(pointerDragEntity.ValueRO.value);

                            if (slotEntity.ValueRO.value == Entity.Null)
                            {
                                if (dragSlotEntity.ValueRO.value != Entity.Null)
                                {
                                    if (SystemAPI.HasComponent<SlotEntityPosition>(dragSlotEntity.ValueRO.value)) SystemAPI.GetComponentRW<SlotEntityPosition>(dragSlotEntity.ValueRO.value).ValueRW.value = slotPosition.ValueRO.value;
                                    else beginEcb.AddComponent(dragSlotEntity.ValueRO.value, new SlotEntityPosition() { value = slotPosition.ValueRO.value });

                                    slotEntity.ValueRW.value = dragSlotEntity.ValueRO.value;
                                    dragSlotEntity.ValueRW.value = Entity.Null;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}

