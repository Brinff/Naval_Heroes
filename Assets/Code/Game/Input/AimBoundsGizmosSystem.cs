using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBoundsGizmosSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupGizmosSystem
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<AimBoundsComponent> m_PoolAimBounds;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<AimBoundsComponent>().End();
        m_PoolAimBounds = m_World.GetPool<AimBoundsComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var aimBounds = ref m_PoolAimBounds.Get(entity);
            using (new GizmosScope(aimBounds.transform.localToWorldMatrix))
            {
                Gizmos.DrawWireCube(aimBounds.bounds.center, aimBounds.bounds.size);
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(aimBounds.enterFaceBounds.center, aimBounds.enterFaceBounds.size);
                Gizmos.DrawWireCube(aimBounds.enterDirectionBounds.center, aimBounds.enterDirectionBounds.size);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(aimBounds.exitBounds.center, aimBounds.exitBounds.size);
            }
        }
    }
}
