using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerEyeRaycastSystem : MonoBehaviour, IEcsInitSystem, IEcsGroupFixedUpdateSystem, IEcsRunSystem
{
    private EcsFilter m_Filter;
    private EcsFilter m_RaycastFilter;
    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<EyeRaycastComponent> m_PoolEyeRaycast;
    private EcsPool<AimBoundsComponent> m_PoolAimBounds;

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        m_Filter = world.Filter<EyeComponent>().Inc<EyeRaycastComponent>().Inc<PlayerTagLeo>().End();
        m_RaycastFilter = world.Filter<AimBoundsComponent>().Exc<PlayerTagLeo>().End();

        m_PoolAimBounds = world.GetPool<AimBoundsComponent>();
        m_PoolEyeRaycast = world.GetPool<EyeRaycastComponent>();
        m_PoolEye = world.GetPool<EyeComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        Plane plane = new Plane(Vector3.up, 0);
        foreach (var entity in m_Filter)
        {
            ref var eye = ref m_PoolEye.Get(entity);
            ref var eyeRaycast = ref m_PoolEyeRaycast.Get(entity);

            eyeRaycast.offset = 120;

            Vector3 forward = eye.rotation * Vector3.forward;
            eyeRaycast.ray = new Ray(eye.position + forward * eyeRaycast.offset, forward);


            bool isRaycastedEntity = FindNearEntity(eyeRaycast.ray, out float entityDistance) != null;
            bool isRaycastedWater = plane.Raycast(eyeRaycast.ray, out float warerDistance);

            eyeRaycast.distance = 2000;


            
            if (isRaycastedEntity) eyeRaycast.distance = entityDistance;
            if (isRaycastedWater) eyeRaycast.distance = warerDistance;
            if (isRaycastedEntity && isRaycastedWater) eyeRaycast.distance = Mathf.Min(warerDistance, entityDistance);

            eyeRaycast.point = eyeRaycast.ray.GetPoint(eyeRaycast.distance);
        }
    }


    public Nullable<int> FindNearEntity(Ray ray, out float minDistance)
    {
        minDistance = 0;
        Nullable<int> nearEntity = null;
        foreach (var entity in m_RaycastFilter)
        {
            ref var aimBounds = ref m_PoolAimBounds.Get(entity);

            Ray localRay = new Ray(aimBounds.transform.InverseTransformPoint(ray.origin), aimBounds.transform.InverseTransformDirection(ray.direction));
            bool hitA = aimBounds.enterFaceBounds.IntersectRay(localRay, out float localDistaneA);
            bool hitB = aimBounds.enterDirectionBounds.IntersectRay(localRay, out float localDistaneB);
            if (hitA || hitB)
            {
                float localDistance = 0;
                if (hitA) localDistance = localDistaneA;
                if (hitB) localDistance = localDistaneB;
                if (hitA && hitB) localDistance = Mathf.Min(localDistaneA, localDistaneB);

                var distance = Vector3.Distance(ray.origin, aimBounds.transform.TransformPoint(localRay.GetPoint(localDistance)));

                if (nearEntity != null)
                {
                    if (minDistance > distance)
                    {
                        nearEntity = entity;
                        minDistance = distance;
                    }
                }
                else
                {
                    nearEntity = entity;
                    minDistance = distance;
                }
            }
        }
        return nearEntity;
    }
}
