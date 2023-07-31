using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PhysicConstrainComponent
{
    public Rigidbody rigidbody;
    public Transform transform;
}

public class PhysicConstrainAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Rigidbody m_Rigidbody;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var physicConstrainComponent = ref ecsWorld.GetPool<PhysicConstrainComponent>().Add(entity);
        physicConstrainComponent.rigidbody = m_Rigidbody;
        physicConstrainComponent.transform = transform;
    }
}
