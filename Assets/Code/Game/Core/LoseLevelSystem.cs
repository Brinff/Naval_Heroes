using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Game.States;
using Code.Services;
using UnityEngine;

public class LoseLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsFilter m_BattleDataFilter;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Exc<DeadTag>().End();
        m_BattleDataFilter = m_World.Filter<BattleData>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (!m_BattleDataFilter.IsAny()) return;

        ref var battleData = ref m_BattleDataFilter.GetSingletonComponent<BattleData>();

        if (!battleData.isStarted || battleData.isEnded) return;

        if (!m_Filter.IsAny())
        {
            battleData.isEnded = true;
            var tempBattleData = battleData;
            ServiceLocator.Get<GameStateMachine>().Play<LoseState>(x=>x.SetBattleData(tempBattleData));
            //var commandSystem = systems.GetSystem<CommandSystem>();
            //var 
            //commandSystem.Execute<GoLoseCommand, BattleData>(m_World.Filter<BattleData>().End().GetSingletonComponent<BattleData>());
        }
    }
}
