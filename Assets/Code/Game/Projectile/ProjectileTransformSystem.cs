using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class ProjectileTransformSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsPool<ProjectileUnityTransform> m_PoolsUnityTransform;
    private EcsPool<ProjectileTransform> m_PoolsTransform;
    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        m_Filter = world.Filter<ProjectileUnityTransform>().Inc<ProjectileTransform>().End();
        m_PoolsUnityTransform = world.GetPool<ProjectileUnityTransform>();
        m_PoolsTransform = world.GetPool<ProjectileTransform>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_Filter)
        {
            var t = m_PoolsTransform.Get(item);
            ref var ut = ref m_PoolsUnityTransform.Get(item);
            ut.transform.position = t.position;
            ut.transform.rotation = t.rotation;
        }
    }
}
