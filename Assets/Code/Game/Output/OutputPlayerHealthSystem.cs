using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class OutputPlayerHealthSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem
{

    private EcsFilter m_Filter;
    //private EcsPool<PlayerTag> m_PoolPlayerTag;
    private EcsPool<HealthComponent> m_PoolHealthComponent;
    private EcsPool<HealthBarComponent> m_PoolHealthBarComponent;

    private PlayerHealthBarWidget m_PlayerHealthBarWidget;

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            var healthComponent = m_PoolHealthComponent.Get(entity);
            if (!m_PoolHealthBarComponent.Has(entity))
            {
                m_PlayerHealthBarWidget.healthBar.SetHealth(healthComponent.currentValue, healthComponent.maxValue);
                ref var healthBarComponent = ref m_PoolHealthBarComponent.Add(entity);
                healthBarComponent.health = healthComponent.currentValue;
            }
            else
            {
                ref var healthBarComponent = ref m_PoolHealthBarComponent.Get(entity);
                if (healthBarComponent.health != healthComponent.currentValue)
                {
                    m_PlayerHealthBarWidget.healthBar.DoHeath(healthComponent.currentValue, healthComponent.maxValue);
                    healthBarComponent.health = healthComponent.currentValue;
                }
            }
        }
    }

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        m_PlayerHealthBarWidget = UISystem.Instance.GetElement<PlayerHealthBarWidget>();

        m_Filter = world.Filter<PlayerTagLeo>().Inc<HealthComponent>().End();
        m_PoolHealthComponent = world.GetPool<HealthComponent>();
        //m_PoolPlayerTag = world.GetPool<PlayerTag>();
        m_PoolHealthBarComponent = world.GetPool<HealthBarComponent>();
    }
}
