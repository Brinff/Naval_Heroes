
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCannonSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<WeaponCannonComponent> m_PoolWeaponCannonComponent;
    private EcsPool<WeaponReloadComponent> m_PoolWeaponReloadComponent;
    private EcsPool<WeaponFireCompoment> m_PoolWeaponFireComponent;
    private EcsPool<WeaponAmmoComponent> m_PoolWeaponAmmoComponent;
    private EcsPool<RootComponent> m_PoolRootComponent;
    private EcsPool<StatScaterComponent> m_PoolStatScater;
    private EcsPool<StatDamageComponent> m_PoolStatDamage;
    private EcsPool<StatProjectileTimeComponent> m_PoolStatProjectileTime;
    private EcsPool<StatProjectileVelocityComponent> m_PoolStatProjectileVelocity;
    private EcsPool<CannonBalisticComponent> m_PoolCannonBalisticComponent;
    private EcsPool<StatFireRandomDelayComponent> m_PoolStatFireRandomDelay;
    private EcsWorld m_World;
    [SerializeField]
    private float m_RandomDelay = 0.3f;

    private VFXCannonShot m_CannonShot;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_CannonShot = systems.GetSystem<PoolSystem>().GetPool<VFXCannonShot>();

        m_Filter = m_World.Filter<WeaponCannonComponent>().Inc<WeaponFireCompoment>().Inc<WeaponReloadComponent>().Inc<WeaponAmmoComponent>().End();
        m_PoolWeaponCannonComponent = m_World.GetPool<WeaponCannonComponent>();
        m_PoolWeaponFireComponent = m_World.GetPool<WeaponFireCompoment>();
        m_PoolWeaponReloadComponent = m_World.GetPool<WeaponReloadComponent>();
        m_PoolWeaponAmmoComponent = m_World.GetPool<WeaponAmmoComponent>();
        m_PoolRootComponent = m_World.GetPool<RootComponent>();
        m_PoolStatScater = m_World.GetPool<StatScaterComponent>();
        m_PoolStatDamage = m_World.GetPool<StatDamageComponent>();
        m_PoolStatProjectileTime = m_World.GetPool<StatProjectileTimeComponent>();
        m_PoolStatProjectileVelocity = m_World.GetPool<StatProjectileVelocityComponent>();
        m_PoolCannonBalisticComponent = m_World.GetPool<CannonBalisticComponent>();
        m_PoolStatFireRandomDelay = m_World.GetPool<StatFireRandomDelayComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var weaponFireComponent = ref m_PoolWeaponFireComponent.Get(entity);
            ref var weaponReloadComponent = ref m_PoolWeaponReloadComponent.Get(entity);
            ref var weaponAmmoComponent = ref m_PoolWeaponAmmoComponent.Get(entity);

            if (weaponAmmoComponent.projectile == null)
            {
                weaponAmmoComponent.projectile = new CannonProjectile()
                {
                    damage = weaponAmmoComponent.ammoData.Damage,
                    timeFactor = 1,
                    velocity = weaponAmmoComponent.ammoData.ProjectileSpeed,
                    scatterAngleMin = weaponAmmoComponent.ammoData.scatterAngleMin,
                    scatterAngleMax = weaponAmmoComponent.ammoData.scatterAngleMax,
                    owner = m_PoolRootComponent.Get(entity).entity
                };

                if (m_PoolRootComponent.Get(entity).entity.Unpack(m_World, out int ownerEntity))
                {
                    if (m_PoolStatScater.Has(ownerEntity))
                    {
                        ref var statScater = ref m_PoolStatScater.Get(ownerEntity);
                        weaponAmmoComponent.projectile.scatterAngleMin *= statScater.factor;
                        weaponAmmoComponent.projectile.scatterAngleMax *= statScater.factor;
                    }

                    if (m_PoolStatDamage.Has(ownerEntity))
                    {
                        ref var statDamage = ref m_PoolStatDamage.Get(ownerEntity);
                        weaponAmmoComponent.projectile.damage *= statDamage.factor;
                    }

                    if (m_PoolStatProjectileTime.Has(ownerEntity))
                    {
                        ref var statProjectileTime = ref m_PoolStatProjectileTime.Get(ownerEntity);
                        weaponAmmoComponent.projectile.timeFactor = Stat.Apply(weaponAmmoComponent.projectile.timeFactor, statProjectileTime.value, statProjectileTime.option);
                    }

                    if (m_PoolStatProjectileVelocity.Has(ownerEntity))
                    {
                        ref var statProjectileVelocity = ref m_PoolStatProjectileVelocity.Get(ownerEntity);
                        weaponAmmoComponent.projectile.velocity = Stat.Apply(weaponAmmoComponent.projectile.velocity, statProjectileVelocity.value, statProjectileVelocity.option);
                    }
                }

                if (m_PoolCannonBalisticComponent.Has(entity))
                {
                    ref var cannonBalisticComponent = ref m_PoolCannonBalisticComponent.Get(entity);
                    cannonBalisticComponent.velocity = weaponAmmoComponent.projectile.velocity;
                }
            }

            if (weaponFireComponent.isFire && weaponFireComponent.isActive && weaponReloadComponent.isRedy)
            {
                if (!weaponFireComponent.isFireNow)
                {
                    float delay = m_RandomDelay;
                    if (m_PoolRootComponent.Get(entity).entity.Unpack(m_World, out int ownerEntity))
                    {
                        if (m_PoolStatFireRandomDelay.Has(ownerEntity))
                        {
                            ref var statFireRandomDelay = ref m_PoolStatFireRandomDelay.Get(ownerEntity);
                            delay = Stat.Apply(delay, statFireRandomDelay.value, statFireRandomDelay.option);
                        }
                    }

                    weaponFireComponent.delay = Random.value * delay;
                    weaponFireComponent.isFireNow = true;
                }
            }

            if (weaponFireComponent.isFireNow && weaponFireComponent.delay > 0) weaponFireComponent.delay -= Time.deltaTime;

            if (weaponFireComponent.isFireNow && weaponFireComponent.delay < 0 && weaponReloadComponent.isRedy)
            {
                weaponFireComponent.isFireNow = false;
                

                var weaponCannonComponent = m_PoolWeaponCannonComponent.Get(entity);

                for (int i = 0; i < weaponCannonComponent.barels.Length; i++)
                {
                    weaponAmmoComponent.projectile.Launch(m_World, weaponCannonComponent.barels[i].position, weaponCannonComponent.barels[i].forward);
                }

                m_CannonShot.Play(weaponCannonComponent.visualEffect.position, weaponCannonComponent.visualEffect.rotation).transform.SetParent(weaponCannonComponent.visualEffect);

                //weaponCannonComponent.visualEffect.Play();

                weaponReloadComponent.isReload = true;
            }
        }
    }
}
