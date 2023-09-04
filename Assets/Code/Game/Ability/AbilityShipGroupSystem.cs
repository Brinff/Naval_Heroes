using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShipGroupSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    //private AbilityWidget m_AbilityWidget;
    private EcsFilter m_AbilityShipFilter;
    private EcsFilter m_AbilityOtherFilter;
    private EcsWorld m_World;
    private AbilityDatabase m_AbilityDatabase;
    private AbilityAmmoDatabase m_AbilityAmmoDatabase;

    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityGroup> m_PoolGroupAbility;
    private EcsPool<RootComponent> m_PoolRoot;
    private EcsPool<Team> m_TeamPool;
    private EcsPool<StatDamageComponent> m_PoolStatDamage;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityAmmoDatabase = systems.GetData<AbilityAmmoDatabase>();

        m_AbilityShipFilter = m_World.Filter<Ability>().Inc<AbilityGroup>().Inc<ShipTag>().Inc<RootComponent>().End();
        m_AbilityOtherFilter = m_World.Filter<Ability>().Inc<WeaponTag>().Inc<RootComponent>().End();

        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolGroupAbility = m_World.GetPool<AbilityGroup>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolStatDamage = m_World.GetPool<StatDamageComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var shipAbilityEntity in m_AbilityShipFilter)
        {
            ref var shipAbility = ref m_PoolAbility.Get(shipAbilityEntity);
            ref var shipAbilityGroup = ref m_PoolGroupAbility.Get(shipAbilityEntity);
            ref var shipAbilityRoot = ref m_PoolRoot.Get(shipAbilityEntity);
            ref var shipAbilityStatDamage = ref m_PoolStatDamage.Get(shipAbilityEntity);

            foreach (var otherAbilityEntity in m_AbilityOtherFilter)
            {
                ref var otherAbility = ref m_PoolAbility.Get(otherAbilityEntity);
                ref var otherAbilityRoot = ref m_PoolRoot.Get(otherAbilityEntity);

                if (!shipAbilityRoot.entity.EqualsTo(in otherAbilityRoot.entity)) continue;

                if (shipAbility.id != otherAbility.id) continue;



                if (shipAbilityGroup.entities == null) shipAbilityGroup.entities = new List<EcsPackedEntity>();
                var abilityEntityPack = m_World.PackEntity(otherAbilityEntity);
                if (!shipAbilityGroup.entities.Contains(abilityEntityPack))
                {
                    shipAbilityGroup.entities.Add(abilityEntityPack);
                }
            }

            foreach (var otherAbilityEntity in m_AbilityOtherFilter)
            {
                ref var otherAbility = ref m_PoolAbility.Get(otherAbilityEntity);
                ref var otherAbilityRoot = ref m_PoolRoot.Get(otherAbilityEntity);
                ref var otherAbilityStatDamage = ref m_PoolStatDamage.Get(otherAbilityEntity);


                if (!shipAbilityRoot.entity.EqualsTo(in otherAbilityRoot.entity)) continue;

                if (shipAbility.id != otherAbility.id) continue;

                otherAbilityStatDamage.value = shipAbilityStatDamage.value / shipAbilityGroup.entities.Count;
            }
        }
    }
}
