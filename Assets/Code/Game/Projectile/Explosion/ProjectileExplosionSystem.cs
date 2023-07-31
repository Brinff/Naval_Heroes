using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public abstract class ProjectileExplosionSystem<T> : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem where T : struct
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<ProjectileExplosionComponent> m_PoolProjectileExplosion;


    private SharedData m_SharedData;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_SharedData = systems.GetShared<SharedData>();
        m_Filter = m_World.Filter<ProjectileExplosionComponent>().Inc<ProjectileDestroyEvent>().Inc<T>().End();
        m_PoolProjectileExplosion = m_World.GetPool<ProjectileExplosionComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var projectileExplosion = ref m_PoolProjectileExplosion.Get(entity);
            Place(m_SharedData, projectileExplosion.position, projectileExplosion.direction);
        }
    }

    protected abstract void Place(SharedData sharedData, Vector3 position, Vector3 direction);
}
