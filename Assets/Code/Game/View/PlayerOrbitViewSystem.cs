using Game.UI;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerOrbitViewSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem
{
    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityHorizontal = 1;
    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityVertical = 1;
    [SerializeField, Range(0, 10)]
    private float m_SensitivityScale = 1;
    [SerializeField, Range(5, 179)]
    private float m_FieldOfView = 60;
    [SerializeField]
    private float m_TiltAngleMin = -10;
    [SerializeField]
    private float m_TiltAngleMax = 10;


    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsFilter m_FilterActive;

    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<OrbitViewComponent> m_PoolOrbitView;
    private EcsPool<OrbitViewActiveEvent> m_PoolOrbitViewAcitveEvent;
    private EcsPool<LookAtViewComponent> m_PoolLookAtView;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();


        m_Filter = m_World.Filter<OrbitViewComponent>().Inc<LookAtViewComponent>().Inc<PlayerTag>().End();
        m_FilterActive = m_World.Filter<OrbitViewComponent>().Inc<LookAtViewComponent>().Inc<PlayerTag>().Inc<OrbitViewActiveEvent>().End();


        m_PoolOrbitView = m_World.GetPool<OrbitViewComponent>();
        m_PoolOrbitViewAcitveEvent = m_World.GetPool<OrbitViewActiveEvent>();
        m_PoolLookAtView = m_World.GetPool<LookAtViewComponent>();
        m_PoolEye = m_World.GetPool<EyeComponent>();
    }

    public Matrix4x4 GetOrbitMatrix(Transform transform)
    {
        Vector3 forward = Vector3.Normalize(Vector3.Cross(transform.right, Vector3.up));
        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(forward) * Quaternion.Euler(90, 0, 0), Vector3.one);
        return matrix;
    }

    public void Run(IEcsSystems systems)
    {

        foreach (var entity in m_FilterActive)
        {
            ref var orbitView = ref m_PoolOrbitView.Get(entity);
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);
            orbitView.eyeEntity = lookAtView.eyeEntity;

            lookAtView.activeView = ViewType.Orbit;
            lookAtView.tiltAngleMin = m_TiltAngleMin;
            lookAtView.tiltAngleMax = m_TiltAngleMax;

            lookAtView.sensitivityVertical = m_SensitivityVertical;
            lookAtView.sensitivityHorizontal = m_SensitivityHorizontal;
            lookAtView.sencitivityScale = m_SensitivityScale;
        }

        foreach (var entity in m_Filter)
        {
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);
            if (lookAtView.activeView != ViewType.Orbit) continue;

            ref var orbitView = ref m_PoolOrbitView.Get(entity);
            Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix(orbitView.transform);
            Matrix4x4 worldToOrbitMatrix = Matrix4x4.Inverse(orbitToWorldMatrix);

            Vector2 targetPosition = worldToOrbitMatrix.MultiplyPoint(lookAtView.position);
            Vector2 directionTarget = Vector3.Normalize(targetPosition - orbitView.focalCenter);

            float2x2 d = new float2x2(new float2(orbitView.focalEllepse.x * orbitView.focalEllepse.x, 0), new float2(0, orbitView.focalEllepse.y * orbitView.focalEllepse.y));
            float2x1 n = new float2x1(math.normalize(new float2(-directionTarget.x, -directionTarget.y)));

            var dm = mathMatrix.mul(d, n);
            float2 p = dm.c0 / math.sqrt(mathMatrix.mul(mathMatrix.mul(mathMatrix.transpose(n), d), n));

            Vector2 directionView = Vector3.Normalize(targetPosition - (Vector2)p);

            var orbitDistance = geometry.rayIntersectEllipse(p, -(float2)directionView, orbitView.orbitElllepse);
            Vector2 cameraPoint = (Vector2)p - directionView * orbitDistance;

            orbitView.position = orbitToWorldMatrix.MultiplyPoint(cameraPoint) + Vector3.up * orbitView.height;
            Vector3 varticalLookAt = Vector3.Normalize(lookAtView.position - orbitView.position);
            Vector3 horizontalLookAt = orbitToWorldMatrix.MultiplyVector(directionView);

            orbitView.rotation = Quaternion.LookRotation(Vector3.Normalize(new Vector3(horizontalLookAt.x, varticalLookAt.y, horizontalLookAt.z)));

            if (lookAtView.eyeEntity.Unpack(m_World, out int eyeEntity))
            {
                ref var eye = ref m_PoolEye.Get(eyeEntity);
                eye.position = orbitView.position;
                eye.rotation = orbitView.rotation;
                eye.fieldOfView = m_FieldOfView;
            }
        }
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_FilterActive)
        {
            m_PoolOrbitViewAcitveEvent.Del(entity);
        }
    }
}
