using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponCannonAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform[] m_Barrles;
    [SerializeField]
    private Transform m_VisualEffect;
    public bool isEnable => gameObject.activeInHierarchy;

    //public void Fire()
    //{
    //    if (IsPossibleFire())
    //    {
    //        var ammo = m_AmmoProvider.GetAmmo();
    //        if (ammo != null)
    //        {
    //            var projectile = ammo.GetProjectile();
    //            if (projectile != null)
    //            {
    //                for (int i = 0; i < m_Barrles.Length; i++)
    //                {
    //                    var barrel = m_Barrles[i];
    //                    projectile.Launch(barrel.position, barrel.forward);
    //                }

    //                m_VisualEffect.Play();
    //                Reload();
    //            }
    //        }
    //    }
    //}

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var weaponCannonComponent = ref ecsWorld.GetPool<WeaponCannonComponent>().Add(entity);
        weaponCannonComponent.barels = m_Barrles;
        weaponCannonComponent.visualEffect = m_VisualEffect;
    }
}
