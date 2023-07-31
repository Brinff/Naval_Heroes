using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
public class ProjectileDamageSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;

    private EcsPool<ProjectileColliderHitComponent> m_PoolProjectileColliderHit;
    private EcsPool<ProjectileDamage> m_PoolProjectileDamage;
    private EcsPool<RootComponent> m_PoolRoot;
    private EcsPool<HealthComponent> m_PoolHealth;
    private EcsPool<ProjectileDestroyEvent> m_PoolPojectileDestoryEvent;
    private EcsPool<ProjectileExplosionComponent> m_PoolProjectileExplosion;
    private EcsPool<SurfaceMetalTag> m_PoolSurfaceMetalTag;
    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<ProjectileColliderHitComponent>().Inc<RootComponent>().End();
        m_PoolProjectileColliderHit = m_World.GetPool<ProjectileColliderHitComponent>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolProjectileDamage = m_World.GetPool<ProjectileDamage>();
        m_PoolHealth = m_World.GetPool<HealthComponent>();
        m_PoolPojectileDestoryEvent = m_World.GetPool<ProjectileDestroyEvent>();
        m_PoolProjectileExplosion = m_World.GetPool<ProjectileExplosionComponent>();
        m_PoolSurfaceMetalTag = m_World.GetPool<SurfaceMetalTag>();
    }

    public void Run(IEcsSystems systems)
    {

        foreach (var entity in m_Filter)
        {
            ref var projectileColliderHit = ref m_PoolProjectileColliderHit.Get(entity);
            float damage = 0;
            for (int i = 0; i < projectileColliderHit.lenght; i++)
            {
                ref var hit = ref projectileColliderHit.hits[i];

                if (!hit.colliderEntity.Unpack(m_World, out int colliderEntity) || colliderEntity != entity) continue;



                if (hit.projectileEntity.Unpack(m_World, out int projectileEntity) && m_PoolProjectileDamage.Has(projectileEntity))
                {
                    ref var projectileDamage = ref m_PoolProjectileDamage.Get(projectileEntity);
                    damage += projectileDamage.damage;

                    if (!m_PoolPojectileDestoryEvent.Has(projectileEntity))
                    {
                        if (m_PoolProjectileExplosion.Has(projectileEntity))
                        {
                            m_PoolSurfaceMetalTag.Add(projectileEntity);
                            ref var projectileExplosion = ref m_PoolProjectileExplosion.Get(projectileEntity);
                            projectileExplosion.position = hit.point;
                            projectileExplosion.direction = hit.direction;
                        }

                       m_PoolPojectileDestoryEvent.Add(projectileEntity);
                    }

                    projectileDamage.damage = 0;
                }
            }

            projectileColliderHit.lenght = 0;


            ref var root = ref m_PoolRoot.Get(entity);

            if (root.entity.Unpack(m_World, out int rootEntity))
            {
                if (m_PoolHealth.Has(rootEntity))
                {
                    ref var health = ref m_PoolHealth.Get(rootEntity);
                    health.currentValue -= damage;
                }
            }
        }



        //if (damage != 0) Debug.Log(damage);
    }
}
