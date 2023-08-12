
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
    [UpdateInGroup(typeof(EndItemGroup))]
    public partial struct ItemOutputMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            foreach (var (itemPosition, pointerUpdateDragEvent, itemEntity) in SystemAPI.Query<RefRW<ItemPosition>, RefRO<PointerUpdateDragEvent>>().WithEntityAccess())
            {
                foreach (var slotOutputData in SystemAPI.Query<RefRO<SlotOutputData>>().WithAll<Slot>())
                {
                    if (slotOutputData.ValueRO.itemEntity == itemEntity)
                    {
                        itemPosition.ValueRW.value = slotOutputData.ValueRO.position;
                    }
                }
            }
        }
    }
}

