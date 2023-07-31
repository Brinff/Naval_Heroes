using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using OceanSystem;

public class BuoyancyFixedUpdateSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupFixedUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<BuoyancyComponent> m_PoolBuoyancyComponent;

    public void Init(IEcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        m_Filter = ecsWorld.Filter<BuoyancyComponent>().End();
        m_PoolBuoyancyComponent = ecsWorld.GetPool<BuoyancyComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            var buoyancyComponent = m_PoolBuoyancyComponent.Get(entity);
            buoyancyComponent.buoyancyController.RunFixedUpdate();
        }
    }
}
