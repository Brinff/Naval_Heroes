using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAmmoReloadSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityAmmoAmount> m_PoolAbilityAmmoAmount;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_PoolAbilityAmmoAmount = m_World.GetPool<AbilityAmmoAmount>();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_Filter = m_World.Filter<AbilityAmmoAmount>().Inc<AbilityReload>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(entity);
            ref var abilityReload = ref m_PoolAbilityReload.Get(entity);
            if (abilityAmmoAmount.current < abilityAmmoAmount.max)
            {
                if (abilityReload.progress >= 1)
                {
                    abilityAmmoAmount.current++;
                    abilityReload.time = 0;
                }
            }
        }
    }
}
