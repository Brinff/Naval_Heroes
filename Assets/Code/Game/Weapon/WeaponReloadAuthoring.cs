using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponReloadAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_ReloadDuration;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponReloadComponent = ref ecsWorld.GetPool<WeaponReloadComponent>().Add(entity);
        weaponReloadComponent.reloadDuration = m_ReloadDuration;
    }
}
