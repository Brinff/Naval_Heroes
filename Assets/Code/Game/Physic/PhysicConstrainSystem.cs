using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class PhysicConstrainSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupFixedUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<PhysicConstrainComponent> m_PoolPhysicConstrainComponent;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        m_Filter = world.Filter<PhysicConstrainComponent>().End();
        m_PoolPhysicConstrainComponent = world.GetPool<PhysicConstrainComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            var physicConstrainComponent = m_PoolPhysicConstrainComponent.Get(entity);
            physicConstrainComponent.transform.rotation = physicConstrainComponent.rigidbody.rotation;
            physicConstrainComponent.transform.position = physicConstrainComponent.rigidbody.position;
        }
    }
}
