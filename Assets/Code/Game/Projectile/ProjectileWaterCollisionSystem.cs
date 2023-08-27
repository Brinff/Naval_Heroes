using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct ProjectileWaterCollision
{

}

public class ProjectileWaterCollisionSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsPool<ProjectileTransform> m_PoolTransform;
    private EcsPool<ProjectileDestroyEvent> m_PoolDestroy;
    private EcsPool<ProjectileExplosionComponent> m_PoolProjectileExplosion;
    private EcsPool<SurfaceWaterTag> m_PoolSurafceWaterTag;
    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();

        m_Filter = world.Filter<ProjectileWaterCollision>().Inc<ProjectileTransform>().End();
        m_PoolDestroy = world.GetPool<ProjectileDestroyEvent>();
        m_PoolTransform = world.GetPool<ProjectileTransform>();
        m_PoolProjectileExplosion = world.GetPool<ProjectileExplosionComponent>();
        m_PoolSurafceWaterTag = world.GetPool<SurfaceWaterTag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            var transform = m_PoolTransform.Get(entity);
            if (transform.position.y < 0)
            {
                if (!m_PoolDestroy.Has(entity))
                {
                    if (m_PoolProjectileExplosion.Has(entity))
                    {
                        m_PoolSurafceWaterTag.Add(entity);
                        ref var projectileExplosion = ref m_PoolProjectileExplosion.Get(entity);
                        projectileExplosion.position = transform.position;
                        projectileExplosion.position.y = 0;
                        projectileExplosion.direction = Vector3.down;
                    }
                    m_PoolDestroy.Add(entity);
                }
            }
        }
    }
}
