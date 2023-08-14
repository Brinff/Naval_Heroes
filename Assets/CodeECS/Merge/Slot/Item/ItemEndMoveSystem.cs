
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
            foreach (var (itemHandleEntity, itemPosition, itemSlotParent, localTransform, pointerUpdateDragEvent, itemEntity) in SystemAPI.Query<RefRO<ItemHandleEntity>, RefRW<ItemPosition>, RefRW<ItemSlotParent>, RefRW<LocalTransform>, RefRO<PointerEndDragEvent>>().WithEntityAccess())
            {
                foreach (var pointerId in SystemAPI.Query<RefRO<PointerId>>())
                {
                    if (PointerHelper.HasFlag(pointerUpdateDragEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        float3 position = localTransform.ValueRO.Position;

                        foreach (var (slotOutputData, slotEntity) in SystemAPI.Query<SlotOutputData>().WithEntityAccess())
                        {
                            if (slotOutputData.itemEntity == itemEntity)
                            {
                                if (slotOutputData.isPosible)
                                {
                                    if (itemSlotParent.ValueRO.value != slotEntity)
                                    {
                                        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
                                        beginEcb.AddComponent(slotEntity, new SlotAddItemEvent() { itemEntity = itemEntity, position = slotOutputData.targetPosition });
                                    }

                                    position = slotOutputData.targetPosition;
                                    endEcb.SetComponent(itemEntity, new LocalTransform() { Position = slotOutputData.targetPosition, Rotation = quaternion.identity, Scale = 1 });
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

