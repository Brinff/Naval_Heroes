using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAimGizmosSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupGizmosSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<TurretAimComponent> m_PoolTurretAim;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<TurretAimComponent>().Inc<TransformComponent>().End();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolTurretAim = m_World.GetPool<TurretAimComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var aimTurret = ref m_PoolTurretAim.Get(entity);
            ref var transform = ref m_PoolTransform.Get(entity);
            Gizmos.DrawLine(transform.transform.position, aimTurret.target);
            Gizmos.DrawSphere(aimTurret.target, 1);
        }
    }
}
