using Unity.Burst;
using Unity.Entities;
using Game.Data.Components;
using Game.Data.Systems;
using Game.Spawn.Components;
using Unity.Transforms;
using Game.Enemy.Components;
using Unity.Collections;

namespace Game.Spawn.Systems
{
    [BurstCompile]
    public partial struct SpawnSystem : ISystem
    {

        public void CopyTag<T>(EntityCommandBuffer ecb, Entity from, Entity to, ref SystemState state) where T : unmanaged, IComponentData
        {

            if(state.EntityManager.HasComponent<T>(from))ecb.AddComponent<T>(to);
        }

        public void CopyComponent<T>(EntityCommandBuffer ecb, Entity from, Entity to, ref SystemState state) where T : unmanaged, IComponentData
        {
            if (state.EntityManager.HasComponent<T>(from)) ecb.AddComponent(to, state.EntityManager.GetComponentData<T>(from));
        }


        public void CopySharedComponent<T>(EntityCommandBuffer ecb, Entity from, Entity to, ref SystemState state) where T : unmanaged, ISharedComponentData
        {
            if (state.EntityManager.HasComponent<T>(from)) ecb.AddSharedComponent(to, state.EntityManager.GetSharedComponent<T>(from));
        }


        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entityDatabase = SystemAPI.GetSingleton<EntityDatabaseSystem.Singleton>();
            foreach (var (entityDataId, localToWorld, spawnerEntity) in SystemAPI.Query<EntityDataId, RefRO<LocalToWorld>>().WithAll<SpawnerTag>().WithNone<SpawnInstanceEntity>().WithEntityAccess())
            {
                if (entityDatabase.HasEntity(entityDataId.value))
                {


                    var prefab = entityDatabase.GetEntity(entityDataId.value);
                    var entity = beginEcb.Instantiate(prefab);
                    beginEcb.SetComponent(entity, new LocalTransform() { Position = localToWorld.ValueRO.Position, Rotation = localToWorld.ValueRO.Rotation, Scale = 1 });

                    CopyTag<EnemyTag>(beginEcb, spawnerEntity, entity, ref state);
                    CopySharedComponent<EntityDataId>(beginEcb, spawnerEntity, entity, ref state);

                    //CopyComponent<EnemyTag>(beginEcb, spawnerEntity, entity, ref state);

                    beginEcb.AddComponent(spawnerEntity, new SpawnInstanceEntity() { value = entity });
                }
            }
        }
    }
}

