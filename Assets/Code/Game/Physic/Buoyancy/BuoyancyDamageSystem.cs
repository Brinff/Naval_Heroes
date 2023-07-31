using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuoyancyDamageSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Fitler;
    private EcsPool<BuoyancyComponent> m_PoolBuoyancy;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Fitler = m_World.Filter<BuoyancyComponent>().Inc<HealthEndEvent>().End();
        m_PoolBuoyancy = m_World.GetPool<BuoyancyComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_Fitler)
        {
            ref var buoyancy = ref m_PoolBuoyancy.Get(item);
            buoyancy.buoyancyController.SetBuoyancy(0);
        }
    }
}
