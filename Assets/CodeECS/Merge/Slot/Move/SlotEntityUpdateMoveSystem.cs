
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Game.Merge.Groups;
using Game.Merge.Components;

using Game.Pointer.Events;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MoveGroup)), UpdateAfter(typeof(SlotEntityBeginMoveSystem))]
    public partial struct SlotEntityUpdateMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (slotEntity, localToWorld, pointerUpdateDragEvent) in SystemAPI.Query<RefRO<SlotEntity>, RefRO<LocalToWorld>, RefRO<PointerUpdateDragEvent>>())
            {
                if (slotEntity.ValueRO.value != Entity.Null)
                {
                    foreach (var (slotEntityPosition, entity) in SystemAPI.Query<RefRW<SlotEntityPosition>>().WithOptions(EntityQueryOptions.FilterWriteGroup).WithEntityAccess())
                    {
                        if (slotEntity.ValueRO.value == entity)
                        {
                            foreach (var (pointerId, pointerRay) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>>())
                            {
                                if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
                                {
                                    if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, localToWorld.ValueRO.Position, localToWorld.ValueRO.Up, out float distance))
                                    {
                                        slotEntityPosition.ValueRW.value = geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance);
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    //if (SystemAPI.HasComponent<SlotEntityPosition>(slotEntity.ValueRO.value))
                    //{

                    //}
                }
            }
        }
    }
}

