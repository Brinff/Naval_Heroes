using System.Collections.Generic;
using Code.Services;
using Code.States;
using Game.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.States
{
    public class BattleState : MonoBehaviour, IPlayState
    {
        [SerializeField] private GameObject m_CommanderPrefab;
        [SerializeField] private MissionDatabase m_MissionDatabase;
        [SerializeField] private GameObject m_BattleVirtualCamera;
        public void OnPlay(IStateMachine stateMachine)
        {
            var entityManager = ServiceLocator.Get<EntityManager>();
            var world = entityManager.world;
            
            var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();
            var commandSystem = entityManager.GetSystem<CommandSystem>();
            /*            var view = world.Filter<ViewComponent>().Inc<BattleTag>().End().GetSingleton();
                        ref var eye = ref world.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
                        eye.view = world.PackEntity(view.Value);*/

            m_BattleVirtualCamera.SetActive(true);

            ServiceLocator.Get<UIController>().GetWidget<CompassWidget>().Clear();

            var playerSlotsSystem = entityManager.GetSystem<PlayerSlotsSystem>();
            playerSlotsSystem.Hide();

            playerSlotsSystem.Bake();

            ServiceLocator.Get<UIController>().compositionModule.Show<UIBattleComposition>();

            var commanderInstance = GameObject.Instantiate(m_CommanderPrefab);
            world.Bake(commanderInstance, out List<int> entities);

            Destroy(commanderInstance);

            var poolCrearBattleTag = world.GetPool<ClearBattleTag>();
            foreach (var item in entities)
            {
                poolCrearBattleTag.Add(item);
            }

            //var playerLevelData = entityManager.GetData<MissionDatabase>();
            var playerLevelProvider = entityManager.GetSystem<PlayerMissionSystem>();

            var levelData = m_MissionDatabase.GetLevel(playerLevelProvider.level);

            var battleDataEntity = world.NewEntity();
            ref var battleData = ref world.GetPool<BattleData>().Add(battleDataEntity);
            battleData.levelData = levelData;
            battleData.stage = 0;
            battleData.level = playerLevelProvider.level;
            battleData.winReward = (int)GameSettings.Instance.winReward.GetResult(playerLevelProvider.level);
            battleData.loseReward = (int)GameSettings.Instance.loseReward.GetResult(playerLevelProvider.level);
            battleData.isShooter = playerLevelProvider.level == 1 && GameSettings.Instance.firstLevelisShooter;
            world.GetPool<ClearBattleTag>().Add(battleDataEntity);

            //SmartlookUnity.Smartlook.TrackNavigationEvent("Battle", SmartlookUnity.Smartlook.NavigationEventType.enter);
            TinySauce.OnGameStarted(battleData.level);

            commandSystem.Execute<GoStageCommand, BattleData>(battleData);

            var buffer = entityManager.GetSystem<BeginEntityCommandSystem>().CreateBuffer();

            var startBattleData = battleData;
            startBattleData.isStarted = true;
            buffer.SetComponent(battleDataEntity, startBattleData);
            //foreach (var entity in filter)
            //{
            //    //ref var playerAimPoint = ref world.GetPool<PlayerAimPointComponent>().AddOrGet(entity);
            //    //playerAimPoint.state = AimState.Aim;

            //    //world.GetPool<OrbitViewActiveEvent>().Add(entity);


            //    //ref var level = ref world.GetPool<LevelComponent>().Add(entity);
            //    //level.data = levelData;
            //    //level.value = playerLevelProvider.level;

            //    //ref var stage = ref world.GetPool<StageComponent>().Add(entity);
            //    //stage.value = 0;

            //    //world.GetPool<LoadStageEvent>().Add(entity);
            //}
        }
    }
}