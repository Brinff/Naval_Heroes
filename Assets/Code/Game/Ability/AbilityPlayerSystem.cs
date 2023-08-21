using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class AbilityPlayerSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private AbilityWidget m_AbilityWidget;
    private EcsFilter m_AbilityCommanderFilter;
    private EcsFilter m_AbilityOtherFilter;
    private EcsWorld m_World;
    private AbilityDatabase m_AbilityDatabase;
    private AbilityAmmoDatabase m_AbilityAmmoDatabase;

    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityAmmo> m_PoolAbilityAmmo;

    private EcsPool<AbilityUI> m_PoolAbilityUI;
    private EcsPool<AbilityAmmoUI> m_PoolAbilityAmmoUI;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityAmmoDatabase = systems.GetData<AbilityAmmoDatabase>();
        m_AbilityWidget = UISystem.Instance.GetElement<AbilityWidget>();


        m_AbilityCommanderFilter = m_World.Filter<Ability>().Inc<AbilityAmmo>().Inc<PlayerTag>().Inc<CommanderTag>().Exc<AbilityUI>().End();
        m_AbilityOtherFilter = m_World.Filter<Ability>().Inc<AbilityAmmo>().Inc<PlayerTag>().Exc<CommanderTag>().Exc<AbilityAmmoUI>().End();

        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityAmmo = m_World.GetPool<AbilityAmmo>();

        m_PoolAbilityUI = m_World.GetPool<AbilityUI>();
        m_PoolAbilityAmmoUI = m_World.GetPool<AbilityAmmoUI>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var commanderAbilityEntity in m_AbilityCommanderFilter)
        {
            ref var commanderAbility = ref m_PoolAbility.Get(commanderAbilityEntity);
            ref var commanderAbilityAmmo = ref m_PoolAbilityAmmo.Get(commanderAbilityEntity);

            var abilityData = m_AbilityDatabase.GetById(commanderAbility.id);
            var abilityAmmoData = m_AbilityAmmoDatabase.GetById(commanderAbilityAmmo.id);

            foreach (var otherAbilityEntity in m_AbilityOtherFilter)
            {
                ref var otherAbility = ref m_PoolAbility.Get(commanderAbilityEntity);



                //var abilityAmmoItem = abilityItem.CreateAmmoItem();
                //abilityAmmoItem.SetSprite(abilityAmmoData.icon);
                //ref var otherAbilityAmmo = ref m_PoolAbilityAmmo.Get(commanderAbilityEntity);
            }

            var abilityItem = m_AbilityWidget.CreateAbility();
            abilityItem.SetAbilityIcon(abilityData.icon);
            abilityItem.SetAmmoIcon(abilityAmmoData.icon);


            ref var abilityUI = ref m_PoolAbilityUI.Add(commanderAbilityEntity);

            abilityUI.abilityItem = abilityItem;


        }
    }
}
