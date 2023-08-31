using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTorpedoAuthoring : MonoBehaviour, IEntityAuthoring
{

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponTorpedo = ref ecsWorld.GetPool<WeaponTorpedo>().Add(entity);
        weaponTorpedo.orgin = transform;
    }
}
