using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData
{
    public int level;
    public LevelData levelData;
    public int stage;
    public int reward;
}

public class GoStageCommand : MonoBehaviour, ICommand<BattleData>, ICommand
{
    private BattleData m_BattleData;

    public void Execute(EcsWorld world, IEcsSystems systems, BattleData context)
    {
        m_BattleData = context;

        if (m_BattleData.stage < m_BattleData.levelData.stages.Count)
        {
            var instance = Instantiate(context.levelData.stages[context.stage]);
            var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();

            foreach (var entity in filter)
            {
                instance.GetComponent<ScriptBehaviour>().Launch(world.PackEntityWithWorld(entity));
                Destroy(instance);
            }
        }
        else
        {
            var filter = world.Filter<PlayerTag>().Inc<ShipTag>().End();
            foreach (var entity in filter)
            {
                world.GetPool<OrbitViewActiveEvent>().Add(entity);
                ref var playerAimPoint = ref world.GetPool<PlayerAimPointComponent>().Get(entity);
                playerAimPoint.state = AimState.Idle;
            }
            var commandSystem = systems.GetSystem<CommandSystem>();
            m_BattleData.reward = m_BattleData.levelData.reward;
            commandSystem.Execute<GoWinCommand, BattleData>(m_BattleData);
        }
    }

    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var commandSystem = systems.GetSystem<CommandSystem>();
        m_BattleData.stage++;
        commandSystem.Execute<GoStageCommand, BattleData>(m_BattleData);
    }
}
