
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Groups;
using Game.Merge.Components;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(BeginItemGroup), OrderFirst = true)]
    public partial struct ItemBeginMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (itemEntity, itemPosition, pointerUpdateDragEvent) in SystemAPI.Query<RefRO<ItemHandleEntity>, RefRW<ItemPosition>, RefRO<PointerBeginDragEvent>>())
            {
                foreach (var (pointerId, pointerRay) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>>())
                {
                    if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        //if (geometry.raycastOnPlane(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, float3.zero, new float3(0, 1, 0), out float distance))
                        //{
                        //    itemPosition.ValueRW.value = geometry.getRayPoint(pointerRay.ValueRO.origin, pointerRay.ValueRO.direction, distance);
                        //}
                        break;
                    }
                }
            }
        }
    }
}

