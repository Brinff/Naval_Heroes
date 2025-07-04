using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCommanderGroupSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    //private AbilityWidget m_AbilityWidget;
    private EcsFilter m_AbilityCommanderFilter;
    private EcsFilter m_AbilityOtherFilter;
    private EcsWorld m_World;
    private AbilityDatabase m_AbilityDatabase;
    private AbilityAmmoDatabase m_AbilityAmmoDatabase;

    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityGroup> m_PoolGroupAbility;
    private EcsPool<Root> m_PoolRoot;
    private EcsPool<Team> m_TeamPool;
    private EcsPool<StatDamageComponent> m_PoolStatDamage;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityAmmoDatabase = systems.GetData<AbilityAmmoDatabase>();

        m_AbilityCommanderFilter = m_World.Filter<Ability>().Inc<AbilityGroup>().Inc<CommanderTag>().End();
        m_AbilityOtherFilter = m_World.Filter<Ability>().Inc<ShipTag>().End();

        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolGroupAbility = m_World.GetPool<AbilityGroup>();
        m_PoolRoot = m_World.GetPool<Root>();
        m_TeamPool = m_World.GetPool<Team>();
        m_PoolStatDamage = m_World.GetPool<StatDamageComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var commanderAbilityEntity in m_AbilityCommanderFilter)
        {
            ref var commanderTeam = ref m_TeamPool.Get(commanderAbilityEntity);
            ref var commanderAbility = ref m_PoolAbility.Get(commanderAbilityEntity);
            ref var commanderAbilityGroup = ref m_PoolGroupAbility.Get(commanderAbilityEntity);
            ref var commanderStatDamage = ref m_PoolStatDamage.Get(commanderAbilityEntity);

            var abilityData = m_AbilityDatabase.GetById(commanderAbility.id);

            foreach (var otherAbilityEntity in m_AbilityOtherFilter)
            {
                ref var otherAbility = ref m_PoolAbility.Get(otherAbilityEntity);

                if (abilityData.id != otherAbility.id) continue;



                ref var root = ref m_PoolRoot.Get(otherAbilityEntity);
                if (root.entity.Unpack(m_World, out int rootEntity))
                {
                    if (m_TeamPool.Has(rootEntity))
                    {
                        ref var team = ref m_TeamPool.Get(rootEntity);
                        if (team.id == commanderTeam.id)
                        {
                            if (commanderAbilityGroup.entities == null) commanderAbilityGroup.entities = new List<EcsPackedEntity>();
                            var abilityEntityPack = m_World.PackEntity(otherAbilityEntity);
                            if (!commanderAbilityGroup.entities.Contains(abilityEntityPack))
                            {
                                ref var abilityStatDamage = ref m_PoolStatDamage.Get(otherAbilityEntity);
                                abilityStatDamage.value *= commanderStatDamage.value;

                                commanderAbilityGroup.entities.Add(abilityEntityPack);
                            }


                        }
                    }
                }
            }
        }
    }
}
