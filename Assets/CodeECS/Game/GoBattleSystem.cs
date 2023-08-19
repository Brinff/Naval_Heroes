
using UnityEngine;
using Unity.Entities;
using Game.UI;
using Game.Levels;
using Unity.Mathematics;
using Game.Mission.Components;
using Unity.Collections;
using Game.Battle.Events;

namespace Game.Battle.Systems
{
    [RequireMatchingQueriesForUpdate]
    public partial class GoBattleSystem : SystemBase
    {
        private StartGameWidget startGameWidget;
        private EntityQuery eventEntityQuery;

        protected override void OnStartRunning()
        {
            startGameWidget = UISystem.Instance.GetElement<StartGameWidget>();
            startGameWidget.SetLevel(SystemAPI.GetSingleton<MissionCurrent>().value);
            startGameWidget.OnClick += OnClickStartGame;

            eventEntityQuery = SystemAPI.QueryBuilder().WithAll<GoBattleEvent>().Build();
        }

        protected override void OnStopRunning()
        {
            startGameWidget.OnClick -= OnClickStartGame;
            startGameWidget = null;
        }

        private void OnClickStartGame()
        {
            var beginEcb = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>().CreateCommandBuffer();

            using var query = EntityManager.CreateEntityQuery(typeof(GoBattleListener));
            using var entities = query.ToEntityArray(Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                beginEcb.AddComponent<GoBattleEvent>(entities[i]);
            }

            using var queryMission = EntityManager.CreateEntityQuery(typeof(MissionSceneData), typeof(MissionCurrent));
            beginEcb.AddComponent(queryMission.GetSingletonEntity(), new MissionLoadRequest() { value = queryMission.GetSingleton<MissionCurrent>().value });
        }

        
        protected override void OnUpdate()
        {
            var endEcb = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
            using var eventEntitias = eventEntityQuery.ToEntityArray(Allocator.Temp);
            for (int i = 0; i < eventEntitias.Length; i++)
            {
                endEcb.RemoveComponent<GoBattleEvent>(eventEntitias[i]);
            }
        }
    }
}
