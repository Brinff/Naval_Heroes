using Game.UI;
using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class LookAtViewSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    private CameraInputWidget m_InputWidget;

    private Vector2 axis;

    private bool m_NeedRanculateLookAtPoint = false;

    public Vector2 GetAxis(float sensitivityHorizontal, float sensitivityVertical, float sensitivityScale)
    {
        return Vector2.Scale(axis, new Vector2(sensitivityHorizontal, sensitivityVertical)) * sensitivityScale * Time.deltaTime;
    }

    private void OnBeginInput(Vector2 delta)
    {
        axis = Vector2.zero;
    }

    private void OnUpdateInput(Vector2 delta)
    {
        axis = delta;
    }

    private void OnEndInput(Vector2 delta)
    {
        axis = Vector2.zero;
        m_NeedRanculateLookAtPoint = true;
    }

    private EcsWorld m_World;
    private EcsFilter m_FilterYesHelper;
    private EcsFilter m_FilterNoHelper;

    //private EcsFilter m_FilterActive;
    private EcsFilter m_FilterEye;
    private EcsFilter m_AimBoundsFilter;

    //private EcsPool<OrbitViewComponent> m_PoolOrbitView;
    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<EyeRaycastComponent> m_PoolEyeRaycast;
    //private EcsPool<OrbitViewActiveEvent> m_PoolOrbitViewAcitveEvent;
    private EcsPool<LookAtViewComponent> m_PoolLookAtView;
    private EcsPool<PlayerAimHelperComponent> m_PoolPlayerAimHelper;
    private EcsPool<AimBoundsComponent> m_PoolAimBounds;
    private EcsPool<DeadTag> m_PoolDead;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();


        m_FilterYesHelper = m_World.Filter<LookAtViewComponent>().Inc<PlayerAimHelperComponent>().Inc<PlayerTagLeo>().End();
        m_FilterNoHelper = m_World.Filter<LookAtViewComponent>().Exc<PlayerAimHelperComponent>().Inc<PlayerTagLeo>().End();
        //m_FilterActive = m_World.Filter<OrbitViewComponent>().Inc<PlayerTag>().Inc<OrbitViewActiveEvent>().End();
        m_AimBoundsFilter = m_World.Filter<AimBoundsComponent>().Exc<PlayerTagLeo>().Exc<DeadTag>().End();

        m_FilterEye = m_World.Filter<EyeComponent>().Inc<PlayerTagLeo>().End();

        m_PoolEye = m_World.GetPool<EyeComponent>();
        m_PoolEyeRaycast = m_World.GetPool<EyeRaycastComponent>();

        m_PoolLookAtView = m_World.GetPool<LookAtViewComponent>();
        m_PoolPlayerAimHelper = m_World.GetPool<PlayerAimHelperComponent>();
        m_PoolAimBounds = m_World.GetPool<AimBoundsComponent>();
        m_PoolDead = m_World.GetPool<DeadTag>();

        m_InputWidget = UISystem.Instance.GetElement<CameraInputWidget>();
        m_InputWidget.OnBeginInput += OnBeginInput;
        m_InputWidget.OnUpdateInput += OnUpdateInput;
        m_InputWidget.OnEndInput += OnEndInput;
    }


    public Nullable<int> FindNearEntity(Ray ray, out float minDistance)
    {
        minDistance = 0;
        Nullable<int> nearEntity = null;
        foreach (var entity in m_AimBoundsFilter)
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

    public Matrix4x4 GetOrbitMatrix(Transform transform)
    {
        Vector3 forward = Vector3.Normalize(Vector3.Cross(transform.right, Vector3.up));
        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(forward) * Quaternion.Euler(90, 0, 0), Vector3.one);
        return matrix;
    }

    public bool ExitAimBounds(Ray ray, int entity)
    {
        ref var aimBounds = ref m_PoolAimBounds.Get(entity);
        Ray localRay = new Ray(aimBounds.transform.InverseTransformPoint(ray.origin), aimBounds.transform.InverseTransformDirection(ray.direction));
        return !aimBounds.exitBounds.IntersectRay(localRay);
    }

    public void CalculateDeltaRotations(int entity, Vector3 point, int targetEntity)
    {
        ref var lookAtView = ref m_PoolLookAtView.Get(entity);
        ref var aimBounds = ref m_PoolAimBounds.Get(targetEntity);
        ref var playerAimHelper = ref m_PoolPlayerAimHelper.Get(entity);

        playerAimHelper.point = aimBounds.transform.InverseTransformPoint(point);


        lookAtView.distance = Vector3.Distance(point, lookAtView.transform.position);
        lookAtView.rotation = Quaternion.LookRotation(Vector3.Normalize(point - lookAtView.transform.position));

        Vector3 direction = Vector3.Normalize(point - lookAtView.transform.position);
        Quaternion lookAtRotationY = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        Vector3 directionY = Quaternion.Inverse(lookAtRotationY) * direction;


        Quaternion lookAtRotationX = Quaternion.LookRotation(directionY);

        Vector3 cameraDirection = lookAtView.rotation * Vector3.forward;



        Quaternion yRotation = Quaternion.LookRotation(new Vector3(cameraDirection.x, 0, cameraDirection.z));

        Vector3 cameraDirectionY = Quaternion.Inverse(yRotation) * cameraDirection;
        Quaternion xRotation = Quaternion.LookRotation(cameraDirectionY);

        playerAimHelper.deltaRotationY = Quaternion.Inverse(yRotation) * lookAtRotationY;
        playerAimHelper.deltaRotationX = Quaternion.Inverse(xRotation) * lookAtRotationX;

        playerAimHelper.offsetRotationY = Quaternion.identity;
        playerAimHelper.offsetRotationX = Quaternion.identity;
    }

    public void RotateGlobal(int entity, Quaternion rotationDeltaY, Quaternion rotationDeltaX)
    {
        ref var lookAtView = ref m_PoolLookAtView.Get(entity);

        Quaternion rotation = lookAtView.rotation;
        rotation = rotationDeltaY * rotation;
        rotation = rotation * rotationDeltaX;

        lookAtView.rotation = QuaternionUtility.ClampAngleX(rotation, lookAtView.tiltAngleMin, lookAtView.tiltAngleMax);

        if (lookAtView.distance == 0) lookAtView.distance = 1000;
    }

    public void RotateAtTarget(int entity, int targetEntity, Quaternion rotationDeltaY, Quaternion rotationDeltaX)
    {
        ref var lookAtView = ref m_PoolLookAtView.Get(entity);
        ref var playerAimHelper = ref m_PoolPlayerAimHelper.Get(entity);

        ref var aimBounds = ref m_PoolAimBounds.Get(targetEntity);


        Vector3 point = aimBounds.transform.TransformPoint(playerAimHelper.point);

        Vector3 direction = Vector3.Normalize(point - lookAtView.transform.position);
        Quaternion lookAtRotationY = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        Vector3 directionY = Quaternion.Inverse(lookAtRotationY) * direction;

        Quaternion lookAtRotationX = Quaternion.LookRotation(directionY);

        playerAimHelper.offsetRotationY = rotationDeltaY * playerAimHelper.offsetRotationY;
        playerAimHelper.offsetRotationX = playerAimHelper.offsetRotationX * rotationDeltaX;

        Quaternion newRotation = (Quaternion.Inverse(playerAimHelper.deltaRotationY) * lookAtRotationY) * (Quaternion.Inverse(playerAimHelper.deltaRotationX) * lookAtRotationX);

        newRotation = playerAimHelper.offsetRotationY * newRotation;

        newRotation = newRotation * playerAimHelper.offsetRotationX;

        lookAtView.rotation = QuaternionUtility.ClampAngleX(newRotation, lookAtView.tiltAngleMin, lookAtView.tiltAngleMax);
        lookAtView.distance = Vector3.Distance(point, lookAtView.transform.position);
    }

    public EcsPackedEntity FindEye()
    {
        foreach (var item in m_FilterEye)
        {
            return m_World.PackEntity(item);
        }
        return new EcsPackedEntity();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_FilterNoHelper)
        {
            m_PoolPlayerAimHelper.Add(entity);
        }

        foreach (var entity in m_FilterYesHelper)
        {
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);

            if (lookAtView.eyeEntity.IsNull(m_World))
            {
                lookAtView.eyeEntity = FindEye();
            }

            Vector3 axis = GetAxis(lookAtView.sensitivityHorizontal, lookAtView.sensitivityVertical, lookAtView.sencitivityScale);

            Quaternion rotationDeltaY = Quaternion.Euler(new Vector3(0, axis.x, 0));
            Quaternion rotationDeltaX = Quaternion.Euler(new Vector3(-axis.y, 0, 0));

            if (lookAtView.eyeEntity.Unpack(m_World, out int eyeEntity))
            {
                ref var eye = ref m_PoolEye.Get(eyeEntity);
                ref var eyeRaycast = ref m_PoolEyeRaycast.Get(eyeEntity);

                ref var playerAimHelper = ref m_PoolPlayerAimHelper.Get(entity);

                if (playerAimHelper.target.IsNull(m_World))
                {
                    RotateGlobal(entity, rotationDeltaY, rotationDeltaX);
                }

                var nearEntity = FindNearEntity(eyeRaycast.ray, out float distance);

                if (!playerAimHelper.target.Unpack(m_World, out int targetEntity) || nearEntity != targetEntity)
                {
                    if (nearEntity != null)
                    {
                        playerAimHelper.target = m_World.PackEntity(nearEntity.Value);
                        m_NeedRanculateLookAtPoint = false;
                        CalculateDeltaRotations(entity, eyeRaycast.point, nearEntity.Value);
                    }
                }

                if (playerAimHelper.target.Unpack(m_World, out int targetEntityExit))
                {

                    if (m_PoolDead.Has(targetEntityExit) || ExitAimBounds(eyeRaycast.ray, targetEntityExit))
                    {
                        Vector3 point = eyeRaycast.ray.GetPoint(1000);
                        Vector3 direction = Vector3.Normalize(point - lookAtView.transform.position);
                        lookAtView.rotation = Quaternion.LookRotation(direction);
                        lookAtView.distance = 1000;
                        playerAimHelper.target = new EcsPackedEntity();
                    }

                }

                if (playerAimHelper.target.Unpack(m_World, out int targetEntityActive))
                {
                    if (m_NeedRanculateLookAtPoint && nearEntity != null)
                    {
                        CalculateDeltaRotations(entity, eyeRaycast.point, targetEntityActive);
                    }
                    m_NeedRanculateLookAtPoint = false;

                    RotateAtTarget(entity, targetEntityActive, rotationDeltaY, rotationDeltaX);
                }
            }

            lookAtView.position = lookAtView.transform.position + lookAtView.rotation * Vector3.forward * lookAtView.distance;
        }

        foreach (var entity in m_FilterNoHelper)
        {
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);

            if (lookAtView.eyeEntity.IsNull(m_World))
            {
                lookAtView.eyeEntity = FindEye();
            }

            Vector3 axis = GetAxis(lookAtView.sensitivityHorizontal, lookAtView.sensitivityVertical, lookAtView.sencitivityScale);

            Quaternion rotationDeltaY = Quaternion.Euler(new Vector3(0, axis.x, 0));
            Quaternion rotationDeltaX = Quaternion.Euler(new Vector3(-axis.y, 0, 0));

            RotateGlobal(entity, rotationDeltaY, rotationDeltaX);

            lookAtView.position = lookAtView.transform.position + lookAtView.rotation * Vector3.forward * lookAtView.distance;
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        if (m_InputWidget != null)
        {
            m_InputWidget.OnBeginInput -= OnBeginInput;
            m_InputWidget.OnUpdateInput -= OnUpdateInput;
            m_InputWidget.OnEndInput -= OnEndInput;
        }
    }
}
