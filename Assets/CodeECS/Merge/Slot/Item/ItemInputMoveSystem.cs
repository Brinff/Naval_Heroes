
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Game.Merge.Groups;
using Game.Merge.Components;

using Game.Pointer.Events;
using UnityEngine.UIElements;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(BeginItemGroup)), UpdateAfter(typeof(ItemBeginMoveSystem))]
    public partial struct ItemInputMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (itemHandleEntity, itemPosition, pointerUpdateDragEvent, itemEntity) in SystemAPI.Query<RefRO<ItemHandleEntity>, RefRW<ItemPosition>, RefRO<PointerUpdateDragEvent>>().WithEntityAccess())
            {
                foreach (var (pointerId, pointerRay) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>>())
                {
                    if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, float3.zero, new float3(0, 1, 0), out float distance))
                        {
                            var point = geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance);
                            itemPosition.ValueRW.value = point + new float3(0, 40, 0);

                            foreach (var slotInputData in SystemAPI.Query<RefRW<SlotInputData>>().WithAll<Slot>())
                            {
                                slotInputData.ValueRW.position = point;
                                slotInputData.ValueRW.itemEntity = itemEntity;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}

