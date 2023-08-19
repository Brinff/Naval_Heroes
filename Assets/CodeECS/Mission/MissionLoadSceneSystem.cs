using Game.Mission.Components;
using Game.Mission.Events;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using UnityEngine;

namespace Game.Mission.Systems
{
    [BurstCompile]
    public partial struct MissionLoadSceneSystem : ISystem
    {
        private EntityQuery loadedEventListenerQuery;
        private EntityQuery loadedEventQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            loadedEventListenerQuery = SystemAPI.QueryBuilder().WithAll<MissionLoadedEventListener>().Build();
            loadedEventQuery = SystemAPI.QueryBuilder().WithAll<MissionLoadedEvent>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (missionLoadRequest, entity) in SystemAPI.Query<RefRO<MissionLoadRequest>>().WithEntityAccess())
            {
                var database = SystemAPI.GetSingletonBuffer<MissionSceneData>(true);
                int index = mathExtention.repeat(missionLoadRequest.ValueRO.value - 1, database.Length);
                var sceneEntity = SceneSystem.LoadSceneAsync(state.WorldUnmanaged, database[index].value, new SceneSystem.LoadParameters() { Flags = SceneLoadFlags.LoadAdditive });
                endEcb.RemoveComponent<MissionLoadRequest>(entity);
                beginEcb.AddComponent(entity, new MissionLoadProgress() { scene = sceneEntity });
            }

            foreach (var (missionLoadProgress, entity) in SystemAPI.Query<RefRO<MissionLoadProgress>>().WithEntityAccess())
            {
                if (SceneSystem.IsSceneLoaded(state.WorldUnmanaged, missionLoadProgress.ValueRO.scene))
                {
                    endEcb.RemoveComponent<MissionLoadProgress>(entity);

                    using var entities = loadedEventListenerQuery.ToEntityArray(Allocator.Temp);
                    for (int i = 0; i < entities.Length; i++)
                    {
                        beginEcb.AddComponent<MissionLoadedEvent>(entities[i]);
                    }
                }
            }

            using var loadedEventEntity = loadedEventQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < loadedEventEntity.Length; i++)
            {
                endEcb.RemoveComponent<MissionLoadedEvent>(loadedEventEntity[i]);
            }
        }
    }
}

