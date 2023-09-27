using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateCommanderSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_AbilityFilter;
    private EcsWorld m_World;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityState> m_PoolAbilityPerfrom;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityFilter = m_World.Filter<AbilityReload>().Inc<AbilityState>().Inc<AbilityGroup>().Inc<CommanderTag>().End();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityPerfrom = m_World.GetPool<AbilityState>();
        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();
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
            ref var abilityGroup = ref m_PoolAbilityGroup.Get(abilityEntity);

            bool hasAnyAmmo = false;
            if (abilityGroup.entities != null)
                for (int i = 0; i < abilityGroup.entities.Count; i++)
                {
                    if (abilityGroup.entities[i].Unpack(m_World, out int childAbility))
                    {
                        ref var childAbilityState = ref m_PoolAbilityPerfrom.Get(childAbility);
                        if (childAbilityState.isAvailable)
                        {
                            hasAnyAmmo = true;
                            break;
                        }
                    }
                }

            abilityState.isAvailable = abilityReload.progress >= 1 && hasAnyAmmo;
            //if (abilityGroup.entities != null)
            //    for (int i = 0; i < abilityGroup.entities.Count; i++)
            //    {
            //        if (abilityGroup.entities[i].Unpack(m_World, out int childAbility))
            //        {
            //            ref var childAbilityState = ref m_PoolAbilityPerfrom.Get(childAbility);
            //            childAbilityState.isZoom = abilityState.isZoom;
            //        }
            //    }

            if (abilityState.isPerfrom)
            {

                if (abilityGroup.entities != null)
                {
                    if (abilityGroup.isSeparately)
                    {
                        for (int i = 0; i < abilityGroup.entities.Count; i++)
                        {
                            if (abilityGroup.entities[i].Unpack(m_World, out int childAbility))
                            {
                                abilityGroup.selector = (int)Mathf.Repeat(abilityGroup.selector, abilityGroup.entities.Count);

                                ref var childAbilityState = ref m_PoolAbilityPerfrom.Get(childAbility);
                                if (childAbilityState.isAvailable)
                                {
                                    if (abilityGroup.selector == i)
                                    {
                                        childAbilityState.isPerfrom = true;
                                        abilityGroup.selector++;
                                        break;
                                    }
                                }
                                else abilityGroup.selector++;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < abilityGroup.entities.Count; i++)
                        {
                            if (abilityGroup.entities[i].Unpack(m_World, out int childAbility))
                            {
                                ref var childAbilityState = ref m_PoolAbilityPerfrom.Get(childAbility);
                                if (childAbilityState.isAvailable)
                                {
                                    childAbilityState.isPerfrom = true;
                                }
                            }
                        }
                    }
                }

                abilityState.isAvailable = false;
                abilityReload.time = 0;
            }


        }
    }
}
