using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoBattleCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var filter = world.Filter<PlayerTagLeo>().Inc<ShipTag>().End();
        var commandSystem = systems.GetSystem<CommandSystem>();
        var sharedData = systems.GetShared<SharedData>();
        foreach (var entity in filter)
        {
            ref var playerAimPoint = ref world.GetPool<PlayerAimPointComponent>().AddOrGet(entity);
            playerAimPoint.state = AimState.Aim;

            world.GetPool<OrbitViewActiveEvent>().Add(entity);

            UISystem.Instance.compositionModule.Show<UIGameShipDefaultComposition>();

            var playerLevelData = sharedData.Get<PlayerLevelData>();
            var playerLevelProvider = sharedData.Get<PlayerLevelProvider>();

            var levelData = playerLevelData.GetLevel(playerLevelProvider.level);

            commandSystem.Execute<GoStageCommand, BattleData>(new BattleData() { levelData = levelData, stage = 0, level = playerLevelProvider.level });

            //ref var level = ref world.GetPool<LevelComponent>().Add(entity);
            //level.data = levelData;
            //level.value = playerLevelProvider.level;

            //ref var stage = ref world.GetPool<StageComponent>().Add(entity);
            //stage.value = 0;

            //world.GetPool<LoadStageEvent>().Add(entity);
        }
    }
}
