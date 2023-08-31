using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTimerDestroySystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<ProjectileTimerDestroy> m_PoolProjectileTimerDestroy;
    private EcsPool<ProjectileDestroyEvent> m_PoolDestroy;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<ProjectileTimerDestroy>().Exc<ProjectileDestroyEvent>().End();
        m_PoolProjectileTimerDestroy = m_World.GetPool<ProjectileTimerDestroy>();
        m_PoolDestroy = m_World.GetPool<ProjectileDestroyEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var timer = ref m_PoolProjectileTimerDestroy.Get(entity);
            if(timer.timer < 0)
            {
                m_PoolDestroy.Add(entity);
            }
            else
            {
                timer.timer -= Time.deltaTime;
            }
        }
    }
}
