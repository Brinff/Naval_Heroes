
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Warships;

public class WeaponTorpedoSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsPool<WeaponTorpedo> m_PoolWeaponTorpedo;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAim> m_PoolAbilityAim;
    private EcsPool<Root> m_PoolRoot;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<StatDamageComponent> m_StatDamage;
    private EcsPool<DeadTag> m_PoolDeadTag;

    //private EcsPool<WeaponReloadComponent> m_PoolWeaponReloadComponent;
    //private EcsPool<WeaponFireCompoment> m_PoolWeaponFireComponent;
    //private EcsPool<WeaponAmmoComponent> m_PoolWeaponAmmoComponent;
    //private EcsPool<RootComponent> m_PoolRootComponent;
    //private EcsPool<StatScaterComponent> m_PoolStatScater;
    //private EcsPool<StatDamageComponent> m_PoolStatDamage;
    //private EcsPool<StatProjectileTimeComponent> m_PoolStatProjectileTime;
    //private EcsPool<StatProjectileVelocityComponent> m_PoolStatProjectileVelocity;
    //private EcsPool<CannonBalisticComponent> m_PoolCannonBalisticComponent;
    //private EcsPool<StatFireRandomDelayComponent> m_PoolStatFireRandomDelay;
    private EcsWorld m_World;
    //[SerializeField]
    //private float m_RandomDelay = 0.3f;

    //private VFXCannonShot m_CannonShot;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        //m_CannonShot = systems.GetSystem<PoolSystem>().GetPool<VFXCannonShot>();
        m_Filter = m_World.Filter<WeaponTorpedo>().Inc<AbilityState>().Inc<AbilityAim>().End();

        ////m_Filter = m_World.Filter<WeaponCannonComponent>().Inc<WeaponFireCompoment>().Inc<WeaponReloadComponent>().Inc<WeaponAmmoComponent>().End();
        m_PoolWeaponTorpedo = m_World.GetPool<WeaponTorpedo>();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAim = m_World.GetPool<AbilityAim>();
        m_PoolRoot = m_World.GetPool<Root>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_StatDamage = m_World.GetPool<StatDamageComponent>();
        m_PoolDeadTag = m_World.GetPool<DeadTag>();

        //m_PoolWeaponFireComponent = m_World.GetPool<WeaponFireCompoment>();
        //m_PoolWeaponReloadComponent = m_World.GetPool<WeaponReloadComponent>();
        //m_PoolWeaponAmmoComponent = m_World.GetPool<WeaponAmmoComponent>();
        //m_PoolRootComponent = m_World.GetPool<RootComponent>();
        //m_PoolStatScater = m_World.GetPool<StatScaterComponent>();
        //m_PoolStatDamage = m_World.GetPool<StatDamageComponent>();
        //m_PoolStatProjectileTime = m_World.GetPool<StatProjectileTimeComponent>();
        //m_PoolStatProjectileVelocity = m_World.GetPool<StatProjectileVelocityComponent>();
        //m_PoolCannonBalisticComponent = m_World.GetPool<CannonBalisticComponent>();
        //m_PoolStatFireRandomDelay = m_World.GetPool<StatFireRandomDelayComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        //var poolSystem = systems.GetSystem<PoolSystem>();
        //var shotVfx = poolSystem.GetPool<VFXCannonShot>();
        foreach (var entity in m_Filter)
        {
            ref var root = ref m_PoolRoot.Get(entity);
            ref var weaponTorpedo = ref m_PoolWeaponTorpedo.Get(entity);
            ref var abilityState = ref m_PoolAbilityState.Get(entity);
            ref var abilityAim = ref m_PoolAbilityAim.Get(entity);



            //Vector3 aimDirection = Ballistics.GetDirection(weaponTorpedo.orgrin.position, abilityAim.point + Vector3.up * 5, 0, 200);

            //weaponTorpedo.aimConstrain.Perfrom(weaponTorpedo.orgrin.position + aimDirection * 10);
            //weaponTorpedo.aimConstrain.SetState(AimState.Aim);

            if (abilityState.isPerfrom)
            {
                int teamId = -1;
                if (root.entity.Unpack(m_World, out int rootEntity))
                {
                    ref var team = ref m_PoolTeam.Get(rootEntity);
                    teamId = team.id;

                    if (m_PoolDeadTag.Has(rootEntity)) continue;
                }

                ref var statDamage = ref m_StatDamage.Get(entity);



                TorpedoProjectile cannonProjectile = new TorpedoProjectile() { damage = statDamage.value, owner = root.entity, target = new Vector3(abilityAim.point.x, -1.5f, abilityAim.point.z), timeFactor = 2, speed = 100, speedLookAt = 1f, team = teamId };

                cannonProjectile.Launch(m_World, new Vector3(weaponTorpedo.orgin.position.x, -1.5f, weaponTorpedo.orgin.position.z), weaponTorpedo.orgin.forward);
                //for (int i = 0; i < weaponTorpedo.barels.Length; i++)
                //{
                //    var barrel = weaponTorpedo.barels[i];
                //    cannonProjectile.Launch(m_World, barrel.position, barrel.forward);
                //}

                //shotVfx.Play(weaponTorpedo.visualEffect.position, weaponTorpedo.visualEffect.rotation);

                //Debug.Log("Weapon Cannon Perfrom: {}");
                abilityState.isPerfrom = false;
            }
        }
        //foreach (var entity in m_Filter)
        //{
        //    ref var weaponFireComponent = ref m_PoolWeaponFireComponent.Get(entity);
        //    ref var weaponReloadComponent = ref m_PoolWeaponReloadComponent.Get(entity);
        //    ref var weaponAmmoComponent = ref m_PoolWeaponAmmoComponent.Get(entity);

        //    if (weaponAmmoComponent.projectile == null)
        //    {
        //        weaponAmmoComponent.projectile = new CannonProjectile()
        //        {
        //            damage = weaponAmmoComponent.ammoData.Damage,
        //            timeFactor = 1,
        //            velocity = weaponAmmoComponent.ammoData.ProjectileSpeed,
        //            scatterAngleMin = weaponAmmoComponent.ammoData.scatterAngleMin,
        //            scatterAngleMax = weaponAmmoComponent.ammoData.scatterAngleMax,
        //            owner = m_PoolRootComponent.Get(entity).entity
        //        };

        //        if (m_PoolRootComponent.Get(entity).entity.Unpack(m_World, out int ownerEntity))
        //        {
        //            if (m_PoolStatScater.Has(ownerEntity))
        //            {
        //                ref var statScater = ref m_PoolStatScater.Get(ownerEntity);
        //                weaponAmmoComponent.projectile.scatterAngleMin *= statScater.factor;
        //                weaponAmmoComponent.projectile.scatterAngleMax *= statScater.factor;
        //            }

        //            if (m_PoolStatDamage.Has(ownerEntity))
        //            {
        //                ref var statDamage = ref m_PoolStatDamage.Get(ownerEntity);
        //                weaponAmmoComponent.projectile.damage *= statDamage.factor;
        //            }

        //            if (m_PoolStatProjectileTime.Has(ownerEntity))
        //            {
        //                ref var statProjectileTime = ref m_PoolStatProjectileTime.Get(ownerEntity);
        //                weaponAmmoComponent.projectile.timeFactor = Stat.Apply(weaponAmmoComponent.projectile.timeFactor, statProjectileTime.value, statProjectileTime.option);
        //            }

        //            if (m_PoolStatProjectileVelocity.Has(ownerEntity))
        //            {
        //                ref var statProjectileVelocity = ref m_PoolStatProjectileVelocity.Get(ownerEntity);
        //                weaponAmmoComponent.projectile.velocity = Stat.Apply(weaponAmmoComponent.projectile.velocity, statProjectileVelocity.value, statProjectileVelocity.option);
        //            }
        //        }

        //        if (m_PoolCannonBalisticComponent.Has(entity))
        //        {
        //            ref var cannonBalisticComponent = ref m_PoolCannonBalisticComponent.Get(entity);
        //            cannonBalisticComponent.velocity = weaponAmmoComponent.projectile.velocity;
        //        }
        //    }

        //    if (weaponFireComponent.isFire && weaponFireComponent.isActive && weaponReloadComponent.isRedy)
        //    {
        //        if (!weaponFireComponent.isFireNow)
        //        {
        //            float delay = m_RandomDelay;
        //            if (m_PoolRootComponent.Get(entity).entity.Unpack(m_World, out int ownerEntity))
        //            {
        //                if (m_PoolStatFireRandomDelay.Has(ownerEntity))
        //                {
        //                    ref var statFireRandomDelay = ref m_PoolStatFireRandomDelay.Get(ownerEntity);
        //                    delay = Stat.Apply(delay, statFireRandomDelay.value, statFireRandomDelay.option);
        //                }
        //            }

        //            weaponFireComponent.delay = Random.value * delay;
        //            weaponFireComponent.isFireNow = true;
        //        }
        //    }

        //    if (weaponFireComponent.isFireNow && weaponFireComponent.delay > 0) weaponFireComponent.delay -= Time.deltaTime;

        //    if (weaponFireComponent.isFireNow && weaponFireComponent.delay < 0 && weaponReloadComponent.isRedy)
        //    {
        //        weaponFireComponent.isFireNow = false;


        //        var weaponCannonComponent = m_PoolWeaponCannonComponent.Get(entity);

        //        for (int i = 0; i < weaponCannonComponent.barels.Length; i++)
        //        {
        //            weaponAmmoComponent.projectile.Launch(m_World, weaponCannonComponent.barels[i].position, weaponCannonComponent.barels[i].forward);
        //        }

        //        m_CannonShot.Play(weaponCannonComponent.visualEffect.position, weaponCannonComponent.visualEffect.rotation).transform.SetParent(weaponCannonComponent.visualEffect);

        //        //weaponCannonComponent.visualEffect.Play();

        //        weaponReloadComponent.isReload = true;
        //    }
        //}
    }
}
