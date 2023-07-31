using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.Rendering;

public class HealthSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_FilterA;
    private EcsFilter m_FilterB;
    private EcsPool<HealthComponent> m_PoolHealth;
    private EcsPool<HealthEndEvent> m_PoolHealthEnd;
    private EcsPool<DeadTag> m_PoolDead;
    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_FilterA = m_World.Filter<HealthComponent>().Exc<HealthEndEvent>().Exc<DeadTag>().End();
        m_FilterB = m_World.Filter<HealthComponent>().Inc<HealthEndEvent>().End();
        m_PoolHealth = m_World.GetPool<HealthComponent>();
        m_PoolHealthEnd = m_World.GetPool<HealthEndEvent>();
        m_PoolDead = m_World.GetPool<DeadTag>();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_FilterB)
        {
            m_PoolHealthEnd.Del(entity);
            m_PoolHealth.Del(entity);
            Debug.Log("End health!");
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_FilterA)
        {
            ref var health = ref m_PoolHealth.Get(entity);
            if (health.currentValue <= 0)
            {
                health.currentValue = 0;
                m_PoolHealthEnd.Add(entity);
                m_PoolDead.Add(entity);                
            }
        }
    }
}
