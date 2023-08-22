using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityStateSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_AbilityFilter;
    private EcsWorld m_World;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityState> m_PoolAbilityPerfrom;
    private EcsPool<AbilityAmmoAmount> m_PoolAbilityAmmoAmount;


    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityFilter = m_World.Filter<AbilityReload>().Inc<AbilityState>().Inc<AbilityAmmoAmount>().End();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityPerfrom = m_World.GetPool<AbilityState>();
        m_PoolAbilityAmmoAmount = m_World.GetPool<AbilityAmmoAmount>();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var abilityPerform = ref m_PoolAbilityPerfrom.Get(abilityEntity);

            abilityPerform.isPerfrom = false;
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var abilityReload = ref m_PoolAbilityReload.Get(abilityEntity);
            ref var abilityState = ref m_PoolAbilityPerfrom.Get(abilityEntity);
            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(abilityEntity);

            abilityState.isAvailable = abilityAmmoAmount.current > 0;

            if (abilityState.isAvailable && abilityState.isPerfrom)
            {
                if (abilityAmmoAmount.current >= abilityAmmoAmount.max) abilityReload.time = 0;
                abilityAmmoAmount.current--;
            }
        }
    }
}
