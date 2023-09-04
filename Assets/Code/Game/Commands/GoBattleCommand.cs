using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoBattleCommand : MonoBehaviour, ICommand
{
    [SerializeField]
    private GameObject m_CommanderPrefab;

    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();
        var commandSystem = systems.GetSystem<CommandSystem>();
        var view = world.Filter<ViewComponent>().Inc<BattleTag>().End().GetSingleton();
        ref var eye = ref world.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
        eye.view = world.PackEntity(view.Value);

        var playerSlotsSystem = systems.GetSystem<PlayerSlotsSystem>();
        playerSlotsSystem.Hide();

        playerSlotsSystem.Bake();

        UISystem.Instance.compositionModule.Show<UIBattleComposition>();

        var commanderInstance = GameObject.Instantiate(m_CommanderPrefab);
        world.Bake(commanderInstance, out List<int> entities);

        Destroy(commanderInstance);

        var poolCrearBattleTag = world.GetPool<ClearBattleTag>();
        foreach (var item in entities)
        {
            poolCrearBattleTag.Add(item);
        }

        var playerLevelData = systems.GetData<MissionDatabase>();
        var playerLevelProvider = systems.GetSystem<PlayerMissionSystem>();

        var levelData = playerLevelData.GetLevel(playerLevelProvider.level);

        var battleDataEntity = world.NewEntity();
        ref var battleData = ref world.GetPool<BattleData>().Add(battleDataEntity);
        battleData.levelData = levelData;
        battleData.stage = 0;
        battleData.level = playerLevelProvider.level;
        battleData.winReward = levelData.reward;
        battleData.loseReward = levelData.loseReward;
        world.GetPool<ClearBattleTag>().Add(battleDataEntity);


        TinySauce.OnGameStarted(battleData.level);

        commandSystem.Execute<GoStageCommand, BattleData>(battleData);

        var buffer = systems.GetSystem<BeginEntityCommandSystem>().CreateBuffer();

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
