
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;

using Game.Data.Components;
using Game.Data.Systems;
using Game.Pointer.Events;

namespace Game.Merge.Systems
{
    [UpdateInGroup(typeof(MergeGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct SlotBuySystem : ISystem
    {
        [BurstCompile]
        public Entity HasItem(Entity slotEntity, ref SystemState state)
        {
            foreach (var (slotItemParent, entity) in SystemAPI.Query<RefRO<ItemSlotParent>>().WithEntityAccess())
            {
                if (slotEntity == slotItemParent.ValueRO.value)
                {
                    return entity;
                }
            }
            return Entity.Null;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (entityDataId, entity) in SystemAPI.Query<EntityDataId>().WithAll<SlotBuyTag, PointerClickEvent>().WithEntityAccess())
            {
                var findEntity = HasItem(entity, ref state);
                if (findEntity != Entity.Null)
                {
                    foreach (var (slotLocalToWorld, slotEntity) in SystemAPI.Query<LocalToWorld>().WithAll<SlotMergeTag>().WithEntityAccess())
                    {
                        if (HasItem(slotEntity, ref state) == Entity.Null)
                        {
                            var itemSlotEntity = SystemAPI.GetComponentRW<ItemSlotParent>(findEntity);
                            itemSlotEntity.ValueRW.value = slotEntity;
                            var itemPositon = SystemAPI.GetComponentRW<ItemPosition>(slotEntity);
                            itemPositon.ValueRW.value = slotLocalToWorld.Position;
                        }
                    }
                }
            }

            foreach (var (entityDataId, localToWorld, entity) in SystemAPI.Query<EntityDataId, RefRO<LocalToWorld>>().WithAll<SlotBuyTag>().WithEntityAccess())
            {
                if (HasItem(entity, ref state) == Entity.Null)
                {
                    var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
                    var database = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();

                    var prefabEntity = database.GetEntity(entityDataId);
                    var handleEntity = beginEcb.Instantiate(prefabEntity);

                    LocalTransform localTransform = new LocalTransform() { Position = localToWorld.ValueRO.Position, Rotation = localToWorld.ValueRO.Rotation, Scale = 1 };

                    beginEcb.SetComponent(handleEntity, localTransform);

                    var itemEntity = beginEcb.CreateEntity();
                    beginEcb.AddComponent(itemEntity, new ItemSlotParent() { value = entity });
                    beginEcb.AddComponent(itemEntity, new ItemPosition() { value = localToWorld.ValueRO.Position });
                    beginEcb.AddComponent(itemEntity, new ItemHandleEntity() { value = handleEntity });

                    beginEcb.AddSharedComponent(itemEntity, entityDataId);

                    beginEcb.AddComponent<PointerHandlerTag>(itemEntity);

                    beginEcb.AddComponent<PointerBeginDragEvent>(itemEntity);
                    beginEcb.SetComponentEnabled<PointerBeginDragEvent>(itemEntity, false);

                    beginEcb.AddComponent<PointerUpdateDragEvent>(itemEntity);
                    beginEcb.SetComponentEnabled<PointerUpdateDragEvent>(itemEntity, false);

                    beginEcb.AddComponent<PointerEndDragEvent>(itemEntity);
                    beginEcb.SetComponentEnabled<PointerEndDragEvent>(itemEntity, false);

                    beginEcb.AddComponent(itemEntity, localTransform);
                    beginEcb.AddComponent<LocalToWorld>(itemEntity);
                    beginEcb.AddComponent(itemEntity, SystemAPI.GetComponent<LocalBounds>(prefabEntity));
                }
            }
        }
    }
}

