using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityReloadSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_AbilityFilter;
    private EcsWorld m_World;
    private EcsPool<AbilityReload> m_PoolAbilityReload;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityFilter = m_World.Filter<AbilityReload>().End();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var abilityReload = ref m_PoolAbilityReload.Get(abilityEntity);

            if(abilityReload.time < abilityReload.duration)
            {
                abilityReload.time += Time.deltaTime;
            }

            abilityReload.progress = abilityReload.time / abilityReload.duration;
        }
    }
}
