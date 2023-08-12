
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;
using Game.Merge.Events;

using Unity.Mathematics;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(EndItemGroup), OrderLast = true)]
    public partial struct ItemEndMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (itemHandleEntity, itemPosition, itemSlotParent, localToWorld, pointerUpdateDragEvent, itemEntity) in SystemAPI.Query<RefRO<ItemHandleEntity>, RefRW<ItemPosition>, RefRW<ItemSlotParent>, RefRO<LocalToWorld>, RefRO<PointerEndDragEvent>>().WithEntityAccess())
            {
                foreach (var pointerId in SystemAPI.Query<RefRO<PointerId>>())
                {
                    if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        float3 position = localToWorld.ValueRO.Position;
                       
                        foreach (var (slotOutputData, slotEntity) in SystemAPI.Query<SlotOutputData>().WithEntityAccess())
                        {
                            if (itemSlotParent.ValueRO.value != slotEntity)
                            {
                                if (slotOutputData.itemEntity == itemEntity)
                                {
                                    if (slotOutputData.isPosible)
                                    {
                                        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
                                        beginEcb.AddComponent(slotEntity, new SlotAddItemEvent() { itemEntity = itemEntity, position = slotOutputData.targetPosition });
                                    }
                                }
                            }

                            endEcb.SetComponent<SlotInputData>(slotEntity, new SlotInputData());
                        }

                        itemPosition.ValueRW.value = position;

                        break;
                    }
                }
            }
        }
    }
}

