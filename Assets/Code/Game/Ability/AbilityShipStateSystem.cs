using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShipStateSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_AbilityFilter;
    private EcsWorld m_World;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityState> m_PoolAbilityPerfrom;
    private EcsPool<AbilityAmmoAmount> m_PoolAbilityAmmoAmount;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityFilter = m_World.Filter<AbilityReload>().Inc<AbilityState>().Inc<AbilityAmmoAmount>().Inc<AbilityGroup>().Inc<ShipTag>().End();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityPerfrom = m_World.GetPool<AbilityState>();
        m_PoolAbilityAmmoAmount = m_World.GetPool<AbilityAmmoAmount>();
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
            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(abilityEntity);
            abilityState.isAvailable = abilityAmmoAmount.current > 0;

            if (abilityState.isAvailable && abilityState.isPerfrom)
            {
                if (abilityAmmoAmount.current >= abilityAmmoAmount.max) abilityReload.time = 0;

                ref var abilityGroup = ref m_PoolAbilityGroup.Get(abilityEntity);
                if (abilityGroup.entities != null)
                {
                    if (abilityGroup.isSeparately)
                    {
                        abilityGroup.selector = (int)Mathf.Repeat(abilityGroup.selector, abilityGroup.entities.Count);
                        if (abilityGroup.entities[abilityGroup.selector].Unpack(m_World, out int shipAbilityChild))
                        {
                            ref var shipAbilityPerfrom = ref m_PoolAbilityPerfrom.Get(shipAbilityChild);
                            shipAbilityPerfrom.isPerfrom = true;
                        }
                        abilityGroup.selector++;
                    }
                    else
                    {
                        for (int i = 0; i < abilityGroup.entities.Count; i++)
                        {
                            if (abilityGroup.entities[i].Unpack(m_World, out int shipAbilityChild))
                            {
                                ref var shipAbilityPerfrom = ref m_PoolAbilityPerfrom.Get(shipAbilityChild);
                                shipAbilityPerfrom.isPerfrom = true;
                            }
                        }
                    }
                }

                abilityAmmoAmount.current--;
            }
        }
    }
}
