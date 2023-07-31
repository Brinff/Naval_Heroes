using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimGizmosSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupGizmosSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<PlayerAimPointComponent> m_PoolAimPoint;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerAimPointComponent>().End();
        m_PoolAimPoint = m_World.GetPool<PlayerAimPointComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var playerAimPoint = ref m_PoolAimPoint.Get(entity);
            Gizmos.DrawLine(Camera.main.transform.position, playerAimPoint.position);
            Gizmos.DrawWireSphere(playerAimPoint.position, 1);
        }
    }
}
