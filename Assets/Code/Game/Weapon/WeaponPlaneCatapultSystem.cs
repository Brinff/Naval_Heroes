using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class WeaponPlaneCatapultSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update> 
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<WeaponPlaneCatapult> m_PoolWeaponPlaneCatapult;
    private EcsPool<AbilityState> m_PoolAbilityState;
    private EcsPool<AbilityAim> m_PoolAbilityAim;
    private EcsPool<RootComponent> m_PoolRoot;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<StatDamageComponent> m_StatDamage;
    private EcsPool<DeadTag> m_PoolDeadTag;

    [SerializeField]
    private GameObject m_Plane;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<WeaponPlaneCatapult>().Inc<AbilityState>().Inc<AbilityAim>().End();
        m_PoolWeaponPlaneCatapult = m_World.GetPool<WeaponPlaneCatapult>();
        m_PoolAbilityState = m_World.GetPool<AbilityState>();
        m_PoolAbilityAim = m_World.GetPool<AbilityAim>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_StatDamage = m_World.GetPool<StatDamageComponent>();
        m_PoolDeadTag = m_World.GetPool<DeadTag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var root = ref m_PoolRoot.Get(entity);
            ref var weaponPlaneCatapult = ref m_PoolWeaponPlaneCatapult.Get(entity);
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

                var planeInstance = Instantiate(m_Plane);
                planeInstance.transform.position = weaponPlaneCatapult.orgin.position;
                planeInstance.transform.rotation = weaponPlaneCatapult.orgin.rotation;

                var rootPlaneEntity = m_World.Bake(planeInstance);


                ref var path = ref m_World.GetPool<MovePath>().Add(rootPlaneEntity);
                ref var teamPlane = ref m_PoolTeam.Add(rootPlaneEntity);
                teamPlane.id = teamId;

                List<float3> pathPoints = new List<float3>();
                pathPoints.Add(weaponPlaneCatapult.orgin.position);
                pathPoints.Add(weaponPlaneCatapult.orgin.position + weaponPlaneCatapult.orgin.forward * 100);
                pathPoints.Add(weaponPlaneCatapult.orgin.position + weaponPlaneCatapult.orgin.forward * 200 + Vector3.up * 50);
                pathPoints.Add(new float3(abilityAim.point.x, pathPoints[2].y, abilityAim.point.z));
                pathPoints.Add(pathPoints[3] + new float3(-400, 0, 0));

                path.target = abilityAim.point;
                path.targetTime = (1f / pathPoints.Count) * 3;
                path.spline = SplineFactory.CreateCatmullRom(pathPoints, false);
                path.damage = statDamage.value;

                //TorpedoProjectile cannonProjectile = new TorpedoProjectile() { damage = statDamage.value, owner = root.entity, target = abilityAim.point, timeFactor = 2, speed = 100, speedLookAt = 1f, team = teamId };

                //cannonProjectile.Launch(m_World, weaponTorpedo.orgin.position, weaponTorpedo.orgin.forward);
                ////for (int i = 0; i < weaponTorpedo.barels.Length; i++)
                ////{
                ////    var barrel = weaponTorpedo.barels[i];
                ////    cannonProjectile.Launch(m_World, barrel.position, barrel.forward);
                ////}

                ////shotVfx.Play(weaponTorpedo.visualEffect.position, weaponTorpedo.visualEffect.rotation);

                ////Debug.Log("Weapon Cannon Perfrom: {}");
                abilityState.isPerfrom = false;
            }
        }
    }
}
