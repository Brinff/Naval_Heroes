
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;

using Game.Data.Components;
using Game.Data.Systems;

namespace Game.Merge.Systems
{
    [UpdateInGroup(typeof(MergeGroup), OrderLast = true)]
    [BurstCompile]
    public partial struct SlotBuyInstanceEntitySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //var buffer = SystemAPI.GetSingletonBuffer<EntityDatabaseItem>(true);
            var database = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();

            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (slotEntity, entityDataId, slotBuy, slotLocalTransform, entity) in SystemAPI.Query<RefRO<SlotEntity>, EntityDataId, RefRO<SlotBuy>, RefRO<LocalTransform>>().WithEntityAccess())
            {

                if (database.HasEntity(entityDataId.value) && slotEntity.ValueRO.value == Entity.Null)
                {
                    var instanceEntity = beginEcb.Instantiate(database.GetEntity(entityDataId));
                    beginEcb.SetComponent(instanceEntity, new LocalTransform() { Position = slotLocalTransform.ValueRO.Position, Rotation = slotLocalTransform.ValueRO.Rotation, Scale = 1 });
                    beginEcb.SetComponent(entity, new SlotEntity() { value = instanceEntity });
                }
            }
        }
    }
}

