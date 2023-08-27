using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public abstract class ProjectileExplosionSystem<T> : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update> where T : struct
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<ProjectileExplosionComponent> m_PoolProjectileExplosion;

    private PoolSystem m_PoolSystem;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_PoolSystem = systems.GetSystem<PoolSystem>();
        m_Filter = m_World.Filter<ProjectileExplosionComponent>().Inc<ProjectileDestroyEvent>().Inc<T>().End();
        m_PoolProjectileExplosion = m_World.GetPool<ProjectileExplosionComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var projectileExplosion = ref m_PoolProjectileExplosion.Get(entity);
            Place(m_PoolSystem, projectileExplosion.position, projectileExplosion.direction);
        }
    }

    protected abstract void Place(PoolSystem pools, Vector3 position, Vector3 direction);
}
