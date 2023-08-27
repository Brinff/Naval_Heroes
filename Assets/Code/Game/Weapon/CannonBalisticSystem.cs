using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Warships;

public class CannonBalisticSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<CannonBalisticComponent> m_PoolCannonBalistic;
    private EcsPool<TurretAimComponent> m_PoolTurretAim;
    private EcsPool<WeaponFireCompoment> m_PoolWeaponFire;
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        m_Filter = world.Filter<CannonBalisticComponent>().Inc<TurretAimComponent>().Inc<WeaponFireCompoment>().End();
        m_PoolCannonBalistic = world.GetPool<CannonBalisticComponent>();
        m_PoolTurretAim = world.GetPool<TurretAimComponent>();
        m_PoolWeaponFire = world.GetPool<WeaponFireCompoment>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var turretAimComponent = ref m_PoolTurretAim.Get(entity);
            ref var cannonBalisticComponent = ref m_PoolCannonBalistic.Get(entity);
            ref var weaponFire = ref m_PoolWeaponFire.Get(entity);
            cannonBalisticComponent.aimDirection = Ballistics.GetDirection(cannonBalisticComponent.orgin.position, turretAimComponent.target, 0, cannonBalisticComponent.velocity);
            cannonBalisticComponent.timeFlight = Ballistics.GetTime(cannonBalisticComponent.aimDirection, cannonBalisticComponent.velocity);

            bool isAngleReady = true;
            //if (cannonBalisticComponent.constrains != null)
            //{
            //    Vector3 aimPoint = cannonBalisticComponent.orgin.position + cannonBalisticComponent.aimDirection;
            //    foreach (var constrain in cannonBalisticComponent.constrains)
            //    {
            //        constrain.SetState(turretAimComponent.state);
            //        constrain.SetAimPoint(aimPoint);
            //        constrain.Perform();
            //        if (!constrain.isReadyAngle)
            //        {
            //            isAngleReady = false;
            //        }
            //    }
            //}

            weaponFire.isActive = isAngleReady;
        }
    }

    //public void SetAimpoint(Vector3 aimPoint)
    //{
    //    m_AimPoint = aimPoint;
    //    m_AimDirection = 
    //    m_TimeFlight = Ballistics.GetTime(m_AimDirection, m_Velocity);
    //    foreach (TurretDirectionConstrain item in m_Constrains)
    //    {
    //        item.SetAimPoint(m_Orgin.position + m_AimDirection);
    //        item.Perform();
    //    }
    //}
}
