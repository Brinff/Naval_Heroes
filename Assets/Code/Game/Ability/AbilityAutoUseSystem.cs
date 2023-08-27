using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class AbilityAutoUseSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAutoUse> m_PoolAbilityAutoUse;
    private BeginEntityCommandSystem m_BeginEntityCommandSystem;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_Filter = m_World.Filter<AbilityAutoUse>().Inc<AbilityState>().End();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAutoUse = m_World.GetPool<AbilityAutoUse>();
    }

    public void Run(IEcsSystems systems)
    {
        var ecb = m_BeginEntityCommandSystem.CreateBuffer();
        foreach (var entity in m_Filter)
        {
            ref var abilityAutoUse = ref m_PoolAbilityAutoUse.Get(entity);
            ref var abilitState = ref m_PoolAbilityState.Get(entity);
            if(abilityAutoUse.isActive && abilitState.isAvailable)
            {
                abilitState.isPerfrom = true;
            }
        }        
    }
}
