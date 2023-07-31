using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBoundsSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
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
            Vector3 delta = aimBounds.transform.position - aimBounds.position;
            Vector3 velocity = delta / Time.deltaTime;

            Vector3 localVelocity = aimBounds.transform.InverseTransformVector(velocity);

            aimBounds.enterFaceBounds.center = aimBounds.bounds.center;
            aimBounds.enterFaceBounds.size = new Vector3(aimBounds.bounds.size.x + aimBounds.enterBoundsExpand, aimBounds.bounds.size.y + aimBounds.enterBoundsExpand, 0);
            //aimBounds.enterFaceBounds.Expand(aimBounds.enterBoundsExpand);

            aimBounds.enterDirectionBounds.center = aimBounds.bounds.center;
            aimBounds.enterDirectionBounds.size = new Vector3(0, aimBounds.bounds.size.y + aimBounds.enterBoundsExpand, aimBounds.bounds.size.z + aimBounds.enterBoundsExpand);
            //aimBounds.enterDirectionBounds.Expand(aimBounds.enterBoundsExpand);

            //aimBounds.enterFaceBounds.Encapsulate(localVelocity + Vector3.Scale(aimBounds.enterFaceBounds.extents, localVelocity.normalized));

            aimBounds.center = aimBounds.transform.TransformPoint(aimBounds.bounds.center);


            aimBounds.exitBounds.center = aimBounds.bounds.center;
            aimBounds.exitBounds.size = aimBounds.bounds.size;

            aimBounds.exitBounds.Expand(aimBounds.exitBoundsExpand);

            aimBounds.position = aimBounds.transform.position;
        }
    }
}
