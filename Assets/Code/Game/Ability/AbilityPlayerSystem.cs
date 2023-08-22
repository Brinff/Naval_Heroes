using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityPlayerSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private AbilityWidget m_AbilityWidget;
    private EcsFilter m_AbilityFilter;
    private EcsFilter m_AbilityOtherFilter;
    private EcsWorld m_World;
    private AbilityDatabase m_AbilityDatabase;
    private AbilityAmmoDatabase m_AbilityAmmoDatabase;

    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityGroup> m_PoolAbilityGroup;
    private EcsPool<AbilityReload> m_PoolAbilityReload;
    private EcsPool<AbilityUI> m_PoolAbilityUI;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAmmoUI> m_PoolAbilityAmmoUI;
    private EcsPool<AbilityAmmoAmount> m_PoolAbilityAmmoAmount;
    private EcsPool<AbilityAmmo> m_PoolAbilityAmmo;
    private BeginEntityCommandSystem m_BeginEntityCommandSystem;

    public void Init(IEcsSystems systems)
    {
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityAmmoDatabase = systems.GetData<AbilityAmmoDatabase>();
        m_AbilityWidget = UISystem.Instance.GetElement<AbilityWidget>();

        m_AbilityFilter = m_World.Filter<Ability>().Inc<AbilityGroup>().Inc<PlayerTag>().Inc<CommanderTag>().End();

        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();
        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityAmmo = m_World.GetPool<AbilityAmmo>();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityUI = m_World.GetPool<AbilityUI>();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAmmoUI = m_World.GetPool<AbilityAmmoUI>();
        m_PoolAbilityAmmoAmount = m_World.GetPool<AbilityAmmoAmount>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var ability = ref m_PoolAbility.Get(abilityEntity);
            ref var abilityGroup = ref m_PoolAbilityGroup.Get(abilityEntity);

            if (abilityGroup.entities != null && abilityGroup.entities.Count > 0)
            {
                if (!m_PoolAbilityUI.Has(abilityEntity))
                {
                    ref var abilityAmmo = ref m_PoolAbilityAmmo.Get(abilityEntity);
                    var abilityData = m_AbilityDatabase.GetById(ability.id);
                    var abilityAmmoData = m_AbilityAmmoDatabase.GetById(abilityAmmo.id);
                    var abilityItem = m_AbilityWidget.CreateAbility();
                    abilityItem.SetAbilityIcon(abilityData.icon);
                    abilityItem.SetId(abilityData.id);
                    abilityItem.SetReload(0);
                    abilityItem.SetAmmoIcon(abilityAmmoData.icon);

                    abilityItem.OnPerform += OnAbilityPerform;


                    foreach (var item in abilityGroup.entities)
                    {


                        if (item.Unpack(m_World, out int childAbility))
                        {
                            ref var abilityAmmoUI = ref m_PoolAbilityAmmoUI.Add(childAbility);
                            abilityAmmoUI.abilityAmmoItem = new List<AmmoItem>();

                            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(childAbility);

                            for (int i = 0; i < abilityAmmoAmount.max; i++)
                            {
                                var ammoItem = abilityItem.CreateAmmoItem();
                                ammoItem.SetReload(abilityAmmoAmount.current > i ? 1 : 0);
                                abilityAmmoUI.abilityAmmoItem.Add(ammoItem);
                            }
                        }
                    }

                    abilityItem.UpdateAmmo();

                    ref var abilityUI = ref m_PoolAbilityUI.Add(abilityEntity);
                    abilityUI.abilityItem = abilityItem;
                }
                else
                {
                    ref var abilityUI = ref m_PoolAbilityUI.Get(abilityEntity);
                    ref var abilityReload = ref m_PoolAbilityReload.Get(abilityEntity);
                    abilityUI.abilityItem.SetReload(abilityReload.progress);

                    foreach (var item in abilityGroup.entities)
                    {
                        if (item.Unpack(m_World, out int childAbility))
                        {
                            ref var abilityAmmoUI = ref m_PoolAbilityAmmoUI.Get(childAbility);
                            ref var abilityAmmoAmount = ref m_PoolAbilityAmmoAmount.Get(childAbility);
                            ref var abilityAmmoReload = ref m_PoolAbilityReload.Get(childAbility);

                            for (int i = 0; i < abilityAmmoAmount.max; i++)
                            {
                                var ammoItem = abilityAmmoUI.abilityAmmoItem[i];

                                //ammoItem.SetReload(abilityAmmoReload.progress);
                                //ammoItem.SetReload(abilityAmmoReload.progress);
                                ammoItem.SetReload(abilityAmmoAmount.current > i ? 1 : abilityAmmoAmount.current == i ? abilityAmmoReload.progress : 0);

                            }
                        }
                    }

                    abilityUI.abilityItem.UpdateAmmo();
                }
            }
        }
    }

    private void OnAbilityPerform(int id)
    {
        var abilityData = m_AbilityDatabase.GetById(id);
        foreach (var abilityEntity in m_AbilityFilter)
        {
            ref var ability = ref m_PoolAbility.Get(abilityEntity);
            ref var abilityState = ref m_PoolAbilityState.Get(abilityEntity);

            if (id == ability.id && abilityState.isAvailable)
            {
                var beginEcb = m_BeginEntityCommandSystem.CreateBuffer();
                beginEcb.SetComponent(abilityEntity, new AbilityState() { isPerfrom = true });
                Debug.Log($"Perform: {abilityData.name}");
            }
        }

    }
}
