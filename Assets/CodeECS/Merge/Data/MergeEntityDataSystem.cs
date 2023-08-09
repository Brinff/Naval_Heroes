
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

using Game.Merge.Components;
using Game.Merge.Groups;

namespace Game.Data.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MergeGroup))]
    public partial struct MergeEntityDataSystem : ISystem
    {
        public struct Singleton : IComponentData
        {
            internal NativeHashMap<int2, int> map;

            public void AddData(int a, int b, int result)
            {
                map.Add(new int2(a, b), result);
            }

            internal void Create(int count, Allocator allocator)
            {
                map = new NativeHashMap<int2, int>(count, allocator);
            }

            public bool Has(int a, int b)
            {
                return map.ContainsKey(new int2(a, b));
            }

            public bool TryResult(int a, int b, out int result)
            {
                int2 has = new int2(a, b);
                if (map.ContainsKey(has))
                {
                    result = map[has];
                    return true;
                }
                result = 0;
                return false;
            }

            public int GetResult(int a, int b)
            {
                return map[new int2(a, b)];
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
            var database = SystemAPI.GetSingleton<Singleton>();
            database.map.Dispose();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var database = SystemAPI.GetSingleton<Singleton>();
            foreach (var item in SystemAPI.Query<DynamicBuffer<MergeEntityData>>())
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (!database.Has(item[i].a, item[i].b)) database.AddData(item[i].a, item[i].b, item[i].result);
                }
            }
        }
    }
}

