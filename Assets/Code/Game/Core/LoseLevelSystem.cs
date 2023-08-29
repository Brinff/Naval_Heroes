using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsFilter m_BattleDataFilter;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Inc<HealthEndEvent>().End();
        m_BattleDataFilter = m_World.Filter<BattleData>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (!m_BattleDataFilter.IsAny()) return;

        ref var battleData = ref m_BattleDataFilter.GetSingletonComponent<BattleData>();

        if (!battleData.isStarted) return;

        if (m_Filter.IsAny())
        {
            var commandSystem = systems.GetSystem<CommandSystem>();
            commandSystem.Execute<GoLoseCommand, BattleData>(m_World.Filter<BattleData>().End().GetSingletonComponent<BattleData>());
        }
    }
}
