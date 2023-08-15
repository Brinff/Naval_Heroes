
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;

using Game.Data.Components;
using Game.Data.Systems;
using System.Runtime.CompilerServices;
using Game.Pointer.Events;

namespace Game.Merge.Systems
{
    [UpdateInGroup(typeof(MergeGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct SlotBuyInstanceEntitySystem : ISystem
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [BurstCompile]
        public bool HasItem(Entity slotEntity, ref SystemState state)
        {
            foreach (var slotItemParent in SystemAPI.Query<RefRO<ItemSlotParent>>())
            {
                if (slotEntity == slotItemParent.ValueRO.value) return true;
            }
            return false;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var item in SystemAPI.Query<EntityDataId>().WithAll<SlotBuyTag, PointerClickEvent>())
            {

            }
            foreach(var (entityDataId, localToWorld, entity) in SystemAPI.Query<EntityDataId, RefRO<LocalToWorld>>().WithAll<SlotBuyTag>().WithEntityAccess())
            {
                if (!HasItem(entity, ref state))
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

