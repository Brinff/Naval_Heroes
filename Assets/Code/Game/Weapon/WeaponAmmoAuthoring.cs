using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warships;

public class WeaponAmmoAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    [SerializeField]
    private ProjectileData m_ProjectileData;
    [SerializeField]
    private int maxCount = 1;
    [SerializeField]
    private int count = 0;
    //[SerializeField]
    //private CannonAmmoData m_AmmoData;
    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponAmmoComponent = ref ecsWorld.GetPool<WeaponAmmoComponent>().Add(entity);
        weaponAmmoComponent.id = m_ProjectileData.id;
        weaponAmmoComponent.count = count;
        weaponAmmoComponent.maxCount = maxCount;
        //weaponAmmoComponent.ammoData = m_AmmoData;
    }
}
