using Unity.Burst;
using Unity.Entities;

using Game.Merge.Components;
using Game.Debug;
using Game.Merge.Groups;
using Game.Pointer.Events;
using Game.Grid;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SlotGroup)), UpdateAfter(typeof(SlotMergeSystem))]
    public partial struct SlotGridSystem : ISystem
    {
        //public bool IsOverlap(GridTransformAspect gridTransformAspect, float3 point)
        //{
        //    foreach (var item in SystemAPI.Query<GridTransformAspect>())
        //    {

        //    } 
        //}


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            //foreach (var (slotGrid, slotGridEnterEvent, entity) in SystemAPI.Query<SlotGrid, RefRO<PointerEnterEvent>>().WithEntityAccess())
            //{
            //    foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
            //    {
            //        if(PointerHelper.HasFlag(slotGridEnterEvent.ValueRO.value, pointerId.ValueRO.value))
            //        {
            //            Debug.DebugSystem.Log("Enter Grid");
            //            var slot = SystemAPI.GetComponent<SlotEntity>(pointerDragEntity.ValueRO.value);
            //            beginEcb.AddComponent(slot.value, new SlotEntityGridPosition());
            //            break;
            //        }
            //    }
            //    //Debug.DebugSystem.Log("Enter Grid");
            //}

            //foreach (var (slotGrid, slotGridExitEvent, entity) in SystemAPI.Query<SlotGrid, RefRO<PointerExitEvent>>().WithEntityAccess())
            //{
            //    //Debug.DebugSystem.Log("Exit Grid");
            //}


            //foreach (var(pointerId, pointerRay, pointerDragEntity, pointerFirstHoveredEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>, RefRO<PointerDragEntity>, RefRO<PointerFirstHoveredEntity>>())
            //{
            //    foreach (var (slotGrid, gridTransfrom, localToWorld, entity) in SystemAPI.Query<SlotGrid, GridTransformAspect, RefRO<LocalToWorld>>().WithEntityAccess())
            //    {
            //        if (entity != pointerFirstHoveredEntity.ValueRO.value) continue;

            //        if (pointerDragEntity.ValueRO.value != Entity.Null && SystemAPI.HasComponent<SlotEntity>(pointerDragEntity.ValueRO.value))
            //        {
            //            var dragSlot = SystemAPI.GetComponentRO<SlotEntity>(pointerDragEntity.ValueRO.value);

            //            foreach (var (slotEntityPosition, entityGridTransform, dragEntity) in SystemAPI.Query<RefRW<SlotEntityPosition>, GridTransformAspect>().WithEntityAccess())
            //            {
            //                if (dragEntity != dragSlot.ValueRO.value) continue;

                            
            //                if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, localToWorld.ValueRO.Position, localToWorld.ValueRO.Up, out float distance))
            //                {
            //                    var point = geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance);
            //                    var matrix = gridTransfrom.GetGridWorldToLocal();
                                
            //                    var rect = entityGridTransform.GetProjectRect(point, matrix);
            //                    Debug.DebugSystem.Log("Draw: " + rect.position + " " + rect.size);

            //                    var projectRects = gridTransfrom.GetRects();
            //                    bool isEnyOverlap = false;
            //                    for (int i = 0; i < projectRects.Length; i++)
            //                    {
            //                        if (projectRects[i].IsOverlap(rect))
            //                        {
            //                            isEnyOverlap = true;
            //                        }
            //                    }
                               
            //                    if (isEnyOverlap)
            //                    {
            //                        slotEntityPosition.ValueRW.value = gridTransfrom.GetGridLocalToWorld().TransformPoint(new float3(rect.position, 0)) + entityGridTransform.GetOffset();
            //                    }
            //                }
            //            }
            //        }

            //        //foreach (var (slotEntityPosition, slotEntityGridPosition, entity) in SystemAPI.Query<RefRW<SlotEntityPosition>, RefRO<SlotEntityGridPosition>>().WithEntityAccess())
            //        //{

            //        //    //if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
            //        //    //{
            //        //    if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, slotLocalToWorld.ValueRO.Position, slotLocalToWorld.ValueRO.Up, out float distance))
            //        //    {
            //        //        slotEntityPosition.ValueRW.value = math.round(geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance) / 30) * 30;
            //        //    }

            //        //    //}
            //        //}

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

