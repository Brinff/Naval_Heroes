using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileColliderHitAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var projectileColliderHitComponent = ref ecsWorld.GetPool<ProjectileColliderHitComponent>().Add(entity);
        projectileColliderHitComponent.hits = new ProjectileHit[10];
        projectileColliderHitComponent.lenght = 0;
    }
}
