


using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Pointer.Data;

using Game.Merge.Components;
using Game.Merge.Groups;

using Game.Data.Systems;
using Game.Data.Components;
using Game.Merge.Events;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SlotGroup))]
    public partial struct SlotMergeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (slotMerge, slotEntity, slotLocalTransform, slotDropEvent, entity) in SystemAPI.Query<SlotMerge, RefRW<SlotEntity>, RefRO<LocalTransform>, RefRO<PointerDropEvent>>().WithEntityAccess())
            {
                foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
                {
                    if (PointerHelper.HasFlag(slotDropEvent.ValueRO.value, pointerId.ValueRO.value))
                    {
                        if (SystemAPI.HasComponent<SlotEntity>(pointerDragEntity.ValueRO.value))
                        {
                            var dragSlotEntity = SystemAPI.GetComponentRW<SlotEntity>(pointerDragEntity.ValueRO.value);

                            if (slotEntity.ValueRO.value != Entity.Null)
                            {
                                if (dragSlotEntity.ValueRO.value != Entity.Null)
                                {
                                    var mergeData = SystemAPI.GetSingleton<MergeEntityDataSystem.Singleton>();
                                    var aId = state.EntityManager.GetSharedComponent<EntityDataId>(slotEntity.ValueRO.value);
                                    var bId = state.EntityManager.GetSharedComponent<EntityDataId>(dragSlotEntity.ValueRO.value);
                                    if(mergeData.TryResult(aId.value, bId.value, out int resultId))
                                    {
                                        var database = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();
                                        //var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

                                        beginEcb.AddComponent(entity, new MergeApplyEvent() { a = slotEntity.ValueRO.value, b = dragSlotEntity.ValueRO.value, result = database.GetEntity(resultId) });

                                        //endEcb.DestroyEntity(slotEntity.ValueRO.value);
                                        //endEcb.DestroyEntity(dragSlotEntity.ValueRO.value);

                                        //var newEntity = beginEcb.Instantiate(database.GetEntity(resultId));
                                        //beginEcb.AddComponent(newEntity, new SlotEntityPosition() { value = slotLocalTransform.ValueRO.Position });
                                        //beginEcb.SetComponent(entity, new SlotEntity() { value = newEntity });

                                        dragSlotEntity.ValueRW.value = Entity.Null;
                                        //Debug.Log("Has Result");
                                    }
                                }
                            }
                            else
                            {
                                if (dragSlotEntity.ValueRO.value != Entity.Null)
                                {
                                    if (SystemAPI.HasComponent<SlotEntityPosition>(dragSlotEntity.ValueRO.value)) SystemAPI.GetComponentRW<SlotEntityPosition>(dragSlotEntity.ValueRO.value).ValueRW.value = slotLocalTransform.ValueRO.Position;
                                    else beginEcb.AddComponent(dragSlotEntity.ValueRO.value, new SlotEntityPosition() { value = slotLocalTransform.ValueRO.Position });

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

