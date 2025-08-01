using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlaneCatapultAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponPlaneCatapult = ref ecsWorld.GetPool<WeaponPlaneCatapult>().Add(entity);
        weaponPlaneCatapult.orgin = transform;
    }
}
