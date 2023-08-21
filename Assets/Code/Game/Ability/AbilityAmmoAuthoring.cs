
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAmmoAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private AbilityAmmoData m_AbilityAmmoData;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var abilityAmmo = ref ecsWorld.GetPool<AbilityAmmo>().Add(entity);
        abilityAmmo.id = m_AbilityAmmoData.id;
    }
}
