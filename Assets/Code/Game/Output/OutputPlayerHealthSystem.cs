using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class OutputPlayerHealthSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroup<Update>
{

    private EcsFilter m_Filter;
    //private EcsPool<PlayerTag> m_PoolPlayerTag;
    private EcsPool<HealthComponent> m_PoolHealthComponent;
    private EcsPool<Team> m_PoolTeam;
    //private EcsPool<HealthBarComponent> m_PoolHealthBarComponent;

    private PlayerHealthBarWidget m_PlayerHealthBarWidget;
    private float m_CurrentHealth;
    private float m_Health;
    public void Run(IEcsSystems systems)
    {
        float currentValue = 0;
        float maxValue = 0;
        foreach (var entity in m_Filter)
        {
            ref var healthComponent = ref m_PoolHealthComponent.Get(entity);
            ref var team = ref m_PoolTeam.Get(entity);

            if (team.id == 0)
            {
                currentValue += healthComponent.currentValue;
                maxValue += healthComponent.maxValue;
            }
            //if (!m_PoolHealthBarComponent.Has(entity))
            //{
            //    m_PlayerHealthBarWidget.healthBar.SetHealth(healthComponent.currentValue, healthComponent.maxValue);
            //    ref var healthBarComponent = ref m_PoolHealthBarComponent.Add(entity);
            //    healthBarComponent.health = healthComponent.currentValue;
            //}
            //else
            //{
            //    ref var healthBarComponent = ref m_PoolHealthBarComponent.Get(entity);

            //}
        }

        if (m_CurrentHealth != currentValue)
        {
            //m_PlayerHealthBarWidget.healthBar.SetHealth(healthComponent.currentValue, healthComponent.maxValue);
            m_PlayerHealthBarWidget.healthBar.DoHeath(currentValue, maxValue);
            m_CurrentHealth = currentValue;
        }
    }

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        m_PlayerHealthBarWidget = UISystem.Instance.GetElement<PlayerHealthBarWidget>();

        m_Filter = world.Filter<Team>().Inc<HealthComponent>().End();
        m_PoolHealthComponent = world.GetPool<HealthComponent>();
        m_PoolTeam = world.GetPool<Team>();
        //m_PoolPlayerTag = world.GetPool<PlayerTag>();
        //m_PoolHealthBarComponent = world.GetPool<HealthBarComponent>();
    }
}
