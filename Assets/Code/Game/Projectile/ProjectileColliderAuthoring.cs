using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileColliderAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var projectileColliderComponent = ref ecsWorld.GetPool<ProjectileColliderComponent>().Add(entity);
        projectileColliderComponent.collider = GetComponent<Collider>();
    }
}
