using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warships;

public class WeaponAmmoAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    [SerializeField]
    private CannonAmmoData m_AmmoData;
    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponAmmoComponent = ref ecsWorld.GetPool<WeaponAmmoComponent>().Add(entity);
        weaponAmmoComponent.ammoData = m_AmmoData;
    }
}
