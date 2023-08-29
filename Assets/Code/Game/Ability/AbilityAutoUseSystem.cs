using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class AbilityAutoUseSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsFilter m_ShipFilter;

    private EcsWorld m_World;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAutoUse> m_PoolAbilityAutoUse;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<DeadTag> m_PoolDeadTag;
    

    private BeginEntityCommandSystem m_BeginEntityCommandSystem;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_Filter = m_World.Filter<AbilityAutoUse>().Inc<AbilityState>().Inc<Team>().End();
        m_ShipFilter = m_World.Filter<ShipTag>().Inc<Team>().Exc<DeadTag>().End();

        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAutoUse = m_World.GetPool<AbilityAutoUse>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolDeadTag = m_World.GetPool<DeadTag>();
        
    }

    public void Run(IEcsSystems systems)
    {
        var ecb = m_BeginEntityCommandSystem.CreateBuffer();
        foreach (var entity in m_Filter)
        {
            ref var abilityAutoUse = ref m_PoolAbilityAutoUse.Get(entity);
            ref var abilitState = ref m_PoolAbilityState.Get(entity);
            ref var abilityTeam = ref m_PoolTeam.Get(entity);

            int countEnemy = 0;
            foreach (var shipEntity in m_ShipFilter)
            {
                ref var shipTeam = ref m_PoolTeam.Get(shipEntity);
                if (shipTeam.id != abilityTeam.id) countEnemy++;
            }

            if(abilityAutoUse.isActive && abilitState.isAvailable)
            {
                abilitState.isPerfrom = countEnemy > 0;
            }
        }        
    }
}
