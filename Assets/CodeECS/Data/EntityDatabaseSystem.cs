using Unity;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

using Game.Data.Components;
using Game.Data.Groups;
using System.Diagnostics;
using static UnityEditor.Experimental.GraphView.Port;

namespace Game.Data.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DataGroup))]
    public partial struct EntityDatabaseSystem : ISystem
    {
        public struct Singleton : IComponentData
        {
            internal NativeHashMap<int, Entity> map;

            public void AddData(int id, Entity entity)
            {
                map.Add(id, entity);
            }

            internal void Create(int count, Allocator allocator)
            {
                map = new NativeHashMap<int, Entity>(count, allocator);
            }

            public Entity GetEntity(EntityDataId entityDataId)
            {
                return GetEntity(entityDataId.value);
            }

            public bool HasEntity(int id)
            {
                return map.ContainsKey(id);
            }

            public Entity GetEntity(int id)
            {
                return map[id];
            }
        }


        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.AddComponent(state.SystemHandle, ComponentType.ReadWrite<Singleton>());
            var query = new EntityQueryBuilder(state.WorldUpdateAllocator).WithAllRW<Singleton>().WithOptions(EntityQueryOptions.IncludeSystems).Build(ref state);
            ref var s = ref query.GetSingletonRW<Singleton>().ValueRW;
            s.Create(10, Allocator.Persistent);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var database = SystemAPI.GetSingleton<Singleton>();
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var item in SystemAPI.Query<DynamicBuffer<EntityDatabase>>())
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (!database.HasEntity(item[i].id))
                    {
                        beginEcb.AddSharedComponent<EntityDataId>(item[i].prefab, new EntityDataId() { value = item[i].id });
                        database.AddData(item[i].id, item[i].prefab);
                    }
                }
            }
        }
    }
}

