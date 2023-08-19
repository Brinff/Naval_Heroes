


using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Pointer.Data;

using Game.Merge.Components;
using Game.Merge.Groups;

using Game.Data.Systems;
using Game.Data.Components;
using Game.Merge.Events;
using Game.Rendering.Material.Components;

using Unity.Mathematics;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SlotGroup))]
    public partial struct SlotMergeSystem : ISystem
    {
        [BurstCompile]
        public bool HasItem(Entity slotEntity, out Entity item, ref SystemState state)
        {
            item = Entity.Null;
            foreach (var (slotItemParent, entity) in SystemAPI.Query<RefRO<ItemSlotParent>>().WithEntityAccess())
            {
                if (slotEntity == slotItemParent.ValueRO.value)
                {
                    item = entity;
                    return true;
                }
            }
            return false;
        }

        //[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var endEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (slotInputData, slotOutputData, slotMegrgeOutputData, worldBounds, localToWorld, gridColor, slotColors, entity) in SystemAPI.Query<RefRO<SlotInputData>, RefRW<SlotOutputData>, RefRW<SlotMergeOutputData>, RefRO<WorldBounds>, RefRO<LocalToWorld>, RefRW<MaterialColorProperty>, RefRO<SlotColors>>().WithAll<SlotMergeTag>().WithEntityAccess())
            {
                if (slotInputData.ValueRO.itemEntity != Entity.Null)
                {
                    slotOutputData.ValueRW.itemEntity = geometry.containsPointInBounds(worldBounds.ValueRO.min, worldBounds.ValueRO.max, slotInputData.ValueRO.position) ? slotInputData.ValueRO.itemEntity : Entity.Null;
                    slotOutputData.ValueRW.targetPosition = localToWorld.ValueRO.Position;

                    if (slotOutputData.ValueRW.itemEntity != Entity.Null)
                    {
                        slotOutputData.ValueRW.position = slotInputData.ValueRO.position;


                        var hasItem = HasItem(entity, out Entity item, ref state);

                        if (hasItem)
                        {
                            var mergeData = SystemAPI.GetSingleton<MergeEntityDataSystem.Singleton>();
                            var aId = state.EntityManager.GetSharedComponent<EntityDataId>(item);
                            var bId = state.EntityManager.GetSharedComponent<EntityDataId>(slotInputData.ValueRO.itemEntity);
                            if (mergeData.TryResult(aId.value, bId.value, out int resultId))
                            {
                                slotOutputData.ValueRW.isPosible = true;

                                slotMegrgeOutputData.ValueRW.itemEntityA = item;
                                slotMegrgeOutputData.ValueRW.itemEntityB = slotInputData.ValueRO.itemEntity;
                                slotMegrgeOutputData.ValueRW.result = resultId;
                            }
                            else
                            {
                                endEcb.SetComponent(entity, new SlotMergeOutputData());
                                slotOutputData.ValueRW.isPosible = false;
                            }
                        }
                        else slotOutputData.ValueRW.isPosible = true;

                        gridColor.ValueRW.value = slotOutputData.ValueRW.isPosible ? slotColors.ValueRO.allow : slotColors.ValueRO.reject;
                    }
                    else
                    {
                        endEcb.SetComponent(entity, new SlotMergeOutputData());
                        slotOutputData.ValueRW.isPosible = false;
                        gridColor.ValueRW.value = slotColors.ValueRO.current;
                    }
                }
                else
                {
                    endEcb.SetComponent(entity, new SlotMergeOutputData());
                    gridColor.ValueRW.value = slotColors.ValueRO.current;
                }

            }

            foreach (var (slotMegrgeOutputData, slotAddItemEvent, entity) in SystemAPI.Query<RefRO<SlotMergeOutputData>, RefRO<SlotAddItemEvent>>().WithAll<SlotMergeTag>().WithEntityAccess())
            {
                var database = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();

                if (slotMegrgeOutputData.ValueRO.itemEntityB != Entity.Null && slotMegrgeOutputData.ValueRO.itemEntityA != Entity.Null && database.HasEntity(slotMegrgeOutputData.ValueRO.result))
                {
                    var prefab = database.GetEntity(slotMegrgeOutputData.ValueRO.result);
                    var instance = beginEcb.Instantiate(prefab);

                    beginEcb.SetComponent(slotMegrgeOutputData.ValueRO.itemEntityB, new ItemHandleEntity() { value = instance });
                    beginEcb.SetComponent(slotMegrgeOutputData.ValueRO.itemEntityB, SystemAPI.GetComponent<LocalBounds>(prefab));
                    beginEcb.SetSharedComponent(slotMegrgeOutputData.ValueRO.itemEntityB, new EntityDataId() { value = slotMegrgeOutputData.ValueRO.result });
                    beginEcb.SetComponent(instance, SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetComponentRO<ItemHandleEntity>(slotMegrgeOutputData.ValueRO.itemEntityB).ValueRO.value));

                    endEcb.DestroyEntity(SystemAPI.GetComponentRO<ItemHandleEntity>(slotMegrgeOutputData.ValueRO.itemEntityB).ValueRO.value);
                    endEcb.DestroyEntity(SystemAPI.GetComponentRO<ItemHandleEntity>(slotMegrgeOutputData.ValueRO.itemEntityA).ValueRO.value);

                    endEcb.DestroyEntity(slotMegrgeOutputData.ValueRO.itemEntityA);
                    endEcb.SetComponent(entity, new SlotMergeOutputData());
                }
            }

            //foreach (var (slotMerge, slotEntity, slotLocalTransform, slotDropEvent, entity) in SystemAPI.Query<SlotMerge, RefRW<SlotEntity>, RefRO<LocalTransform>, RefRO<PointerDropEvent>>().WithEntityAccess())
            //{
            //    foreach (var (pointerId, pointerDragEntity) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerDragEntity>>())
            //    {
            //        if (PointerHelper.HasFlag(slotDropEvent.ValueRO.value, pointerId.ValueRO.value))
            //        {
            //            if (SystemAPI.HasComponent<SlotEntity>(pointerDragEntity.ValueRO.value))
            //            {
            //                var dragSlotEntity = SystemAPI.GetComponentRW<SlotEntity>(pointerDragEntity.ValueRO.value);

            //                if (slotEntity.ValueRO.value != Entity.Null)
            //                {
            //                    if (dragSlotEntity.ValueRO.value != Entity.Null)
            //                    {
            //                        var mergeData = SystemAPI.GetSingleton<MergeEntityDataSystem.Singleton>();
            //                        var aId = state.EntityManager.GetSharedComponent<EntityDataId>(slotEntity.ValueRO.value);
            //                        var bId = state.EntityManager.GetSharedComponent<EntityDataId>(dragSlotEntity.ValueRO.value);
            //                        if(mergeData.TryResult(aId.value, bId.value, out int resultId))
            //                        {
            //                            var database = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();
            //                            //var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            //                            beginEcb.AddComponent(entity, new MergeApplyEvent() { a = slotEntity.ValueRO.value, b = dragSlotEntity.ValueRO.value, result = database.GetEntity(resultId) });

            //                            //endEcb.DestroyEntity(slotEntity.ValueRO.value);
            //                            //endEcb.DestroyEntity(dragSlotEntity.ValueRO.value);

            //                            //var newEntity = beginEcb.Instantiate(database.GetEntity(resultId));
            //                            //beginEcb.AddComponent(newEntity, new SlotEntityPosition() { value = slotLocalTransform.ValueRO.Position });
            //                            //beginEcb.SetComponent(entity, new SlotEntity() { value = newEntity });

            //                            dragSlotEntity.ValueRW.value = Entity.Null;
            //                            //Debug.Log("Has Result");
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if (dragSlotEntity.ValueRO.value != Entity.Null)
            //                    {
            //                        if (SystemAPI.HasComponent<SlotEntityPosition>(dragSlotEntity.ValueRO.value)) SystemAPI.GetComponentRW<SlotEntityPosition>(dragSlotEntity.ValueRO.value).ValueRW.value = slotLocalTransform.ValueRO.Position;
            //                        else beginEcb.AddComponent(dragSlotEntity.ValueRO.value, new SlotEntityPosition() { value = slotLocalTransform.ValueRO.Position });

            //                        slotEntity.ValueRW.value = dragSlotEntity.ValueRO.value;
            //                        dragSlotEntity.ValueRW.value = Entity.Null;
            //                    }
            //                }
            //            }
            //            break;
            //        }
            //    }
            //}
        }
    }
}

