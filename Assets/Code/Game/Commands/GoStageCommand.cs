using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ClearBattleTag
{

}

public struct BattleData
{
    public bool isStarted;
    public bool isEnded;
    public int level;
    public LevelData levelData;
    public int stage;
    public int winReward;
    public int loseReward;
    public List<GameObject> enemies;
}

public class GoStageCommand : MonoBehaviour, ICommand<BattleData>, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems, BattleData battleData)
    {
        Debug.Log("Launch " + battleData.stage);
        //m_BattleData = context;
        //ref var battleData = ref world.Filter<BattleData>().End().GetSingletonComponent<BattleData>();
        if (battleData.stage < battleData.levelData.stages.Count)
        {
            Debug.Log("Instance");
            var instance = Instantiate(battleData.levelData.stages[battleData.stage]);
            world.Bake(instance, out List<int> entitieas);
            Destroy(instance);

            var poolClearBattleTag = world.GetPool<ClearBattleTag>();
            foreach (var item in entitieas)
            {
                poolClearBattleTag.Add(item);
            }


            //var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();

            //foreach (var entity in filter)
            //{
            //    instance.GetComponent<ScriptBehaviour>().Launch(world.PackEntityWithWorld(entity));
            //    Destroy(instance);
            //}
        }
        else
        {
            //var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();
            //foreach (var entity in filter)
            //{
            //    world.GetPool<OrbitViewActiveEvent>().Add(entity);
            //    ref var playerAimPoint = ref world.GetPool<PlayerAimPointComponent>().Get(entity);
            //    playerAimPoint.state = AimState.Idle;
            //}
            var commandSystem = systems.GetSystem<CommandSystem>();
            commandSystem.Execute<GoWinCommand, BattleData>(battleData);
        }
    }

    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var commandSystem = systems.GetSystem<CommandSystem>();
        ref var battleData = ref world.Filter<BattleData>().End().GetSingletonComponent<BattleData>();
        battleData.stage++;
        commandSystem.Execute<GoStageCommand, BattleData>(battleData);
    }
}
