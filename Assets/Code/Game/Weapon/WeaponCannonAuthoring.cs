using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponCannonAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private ProjectileData m_ProjectileData;
    [SerializeField]
    private Transform[] m_Barrles;
    [SerializeField]
    private Transform m_Orgin;
    [SerializeField]
    private TurretAimConstrain m_TurretAimConstrain;
    [SerializeField]
    private Transform m_VisualEffect;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponCannonComponent = ref ecsWorld.GetPool<WeaponCannon>().Add(entity);
        weaponCannonComponent.orgrin = m_Orgin;
        weaponCannonComponent.aimConstrain = m_TurretAimConstrain;
        weaponCannonComponent.projectile = m_ProjectileData.id;
        weaponCannonComponent.barels = m_Barrles;
        weaponCannonComponent.visualEffect = m_VisualEffect;
    }
}
