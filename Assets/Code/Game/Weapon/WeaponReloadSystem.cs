using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public class WeaponReloadSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<WeaponReloadComponent> m_PoolWeaponReloadComponent;
    private EcsPool<StatReloadComponent> m_PoolStatReloadComponent;
    private EcsPool<RootComponent> m_PoolRoot;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<WeaponReloadComponent>().End();
        m_PoolWeaponReloadComponent = m_World.GetPool<WeaponReloadComponent>();
        m_PoolStatReloadComponent = m_World.GetPool<StatReloadComponent>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolStatReloadComponent = m_World.GetPool<StatReloadComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        float deltaTime = Time.deltaTime;
        foreach (var entity in m_Filter)
        {
            ref var weaponReloadComponent = ref m_PoolWeaponReloadComponent.Get(entity);

            if (weaponReloadComponent.isRedy == false)
            {
                if (weaponReloadComponent.reloadTime <= 0)
                {
                    weaponReloadComponent.modifyReloadDuration = weaponReloadComponent.reloadDuration;
                    weaponReloadComponent.isRedy = true;
                    weaponReloadComponent.isReload = false;
                }

                if (weaponReloadComponent.reloadTime > 0)
                {
                    weaponReloadComponent.reloadTime -= deltaTime;
                }
            }

            if (weaponReloadComponent.isReload)
            {
                if (weaponReloadComponent.isRedy == true)
                {
                    float reloadDuration = weaponReloadComponent.reloadDuration;
                    if (m_PoolRoot.Has(entity))
                    {
                        ref var root = ref m_PoolRoot.Get(entity);
                        if (root.entity.Unpack(m_World, out int rootEntity))
                        {
                            if (m_PoolStatReloadComponent.Has(rootEntity))
                            {
                                ref var statReload = ref m_PoolStatReloadComponent.Get(rootEntity);
                                reloadDuration = statReload.reloadDuration;
                            }
                        }
                    }

                    weaponReloadComponent.reloadTime = weaponReloadComponent.modifyReloadDuration = reloadDuration;
                }
                weaponReloadComponent.isRedy = false;
            }

            weaponReloadComponent.progress = 1 - (weaponReloadComponent.reloadTime / weaponReloadComponent.modifyReloadDuration);
        }
    }

    //public bool IsPossibleFire()
    //{
    //    return 0 >= m_ReloadTime;
    //}
    //public void Reload()
    //{
    //    m_ReloadTime = m_ReloadDuration;
    //}

    //public bool IsReload()
    //{
    //    return m_ReloadTime > 0;
    //}

    //public void Update()
    //{
    //    if (IsReload()) m_ReloadTime -= Time.deltaTime;
    //}
}
