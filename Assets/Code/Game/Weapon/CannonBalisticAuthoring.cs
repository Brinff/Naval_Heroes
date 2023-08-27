using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warships;

public struct CannonBalisticComponent
{
    public Transform orgin;
    public float velocity;
    public Vector3 aimDirection;
    public float timeFlight;
    //public TurretDirectionConstrain[] constrains;
}

public class CannonBalisticAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    [SerializeField]
    private Transform m_Orgin;

    //[SerializeField]
    //private TurretDirectionConstrain[] m_Constrains;

    [SerializeField]
    private float m_Velocity;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var cannonBalisticComponent = ref ecsWorld.GetPool<CannonBalisticComponent>().Add(entity);
        cannonBalisticComponent.velocity = m_Velocity;
        //cannonBalisticComponent.constrains = m_Constrains;
        cannonBalisticComponent.orgin = m_Orgin;
    }
}
