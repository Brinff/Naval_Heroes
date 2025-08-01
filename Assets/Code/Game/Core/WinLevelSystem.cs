using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_NewFilter;
    private EcsFilter m_KillFilter;
    private EcsFilter m_BattleDataFilter;
    private EcsFilter m_PlayerAliveFilter;
    private EcsFilter m_PlayerDeadFilter;

    //[SerializeField]
    //private List<EcsPackedEntity> m_Enemies = new List<EcsPackedEntity>();

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_NewFilter = m_World.Filter<NewEntityTag>().Inc<ShipTag>().Exc<PlayerTag>().End();
        m_KillFilter = m_World.Filter<DeadTag>().Inc<ShipTag>().Inc<Team>().Exc<PlayerTag>().End();
        m_BattleDataFilter = m_World.Filter<BattleData>().End();
        m_PlayerAliveFilter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Exc<DeadTag>().End();
        m_PlayerDeadFilter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Inc<DeadTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (!m_BattleDataFilter.IsAny()) return;

        ref var battleData = ref m_BattleDataFilter.GetSingletonComponent<BattleData>();

        if (!battleData.isStarted || battleData.isEnded) return;


        if (m_KillFilter.GetEntitiesCount() == battleData.enemies.Count)
        {
            Debug.Log("Killed all: end game!");
            if (m_PlayerAliveFilter.IsAny())
            {
                Debug.Log("Try no next stage");
                battleData.isEnded = true;
                systems.GetSystem<CommandSystem>().Execute<GoStageCommand>();
            }
        }
    }
}
