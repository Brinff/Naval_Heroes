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
    private EcsPool<AbilityPerfrom> m_PoolAbilityPerfrom;

    private BeginEntityCommandSystem m_BeginEntityCommandSystem;

    public void Init(IEcsSystems systems)
    {
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_World = systems.GetWorld();
        m_AbilityDatabase = systems.GetData<AbilityDatabase>();
        m_AbilityWidget = UISystem.Instance.GetElement<AbilityWidget>();

        m_AbilityFilter = m_World.Filter<Ability>().Inc<AbilityGroup>().Inc<PlayerTag>().Inc<CommanderTag>().End();

        m_PoolAbilityGroup = m_World.GetPool<AbilityGroup>();
        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityReload = m_World.GetPool<AbilityReload>();
        m_PoolAbilityUI = m_World.GetPool<AbilityUI>();
        m_PoolAbilityPerfrom = m_World.GetPool<AbilityPerfrom>();
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
                    var abilityData = m_AbilityDatabase.GetById(ability.id);

                    var abilityItem = m_AbilityWidget.CreateAbility();
                    abilityItem.SetAbilityIcon(abilityData.icon);
                    abilityItem.SetId(abilityData.id);
                    abilityItem.SetReload(0);
                    abilityItem.OnPerform += OnAbilityPerform;

                    ref var abilityUI = ref m_PoolAbilityUI.Add(abilityEntity);
                    abilityUI.abilityItem = abilityItem;
                }
                else
                {
                    ref var abilityUI = ref m_PoolAbilityUI.Get(abilityEntity);
                    ref var abilityReload = ref m_PoolAbilityReload.Get(abilityEntity);
                    abilityUI.abilityItem.SetReload(abilityReload.progress);
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
            if (id == ability.id)
            {
                var beginEcb = m_BeginEntityCommandSystem.CreateBuffer();
                if (m_PoolAbilityPerfrom.Has(abilityEntity)) beginEcb.SetComponent(abilityEntity, new AbilityPerfrom() { isPerfrom = true });
                else beginEcb.AddComponent(abilityEntity, new AbilityPerfrom() { isPerfrom = true });
            }
        }
        Debug.Log($"Perform: {abilityData.name}");
    }
}
