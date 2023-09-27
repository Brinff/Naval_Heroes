using Game.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

//public struct OrbitViewComponent
//{
//    public EyeView eyeView;
//}

//public struct ZoomViewComponent
//{
//    public EyeView eyeView;
//}

public class InputPlayerZoomSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{

    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityHorizontal = 1;
    [SerializeField, Range(0f, 10f)]
    private float m_SensitivityVertical = 1;
    [SerializeField, Range(0, 10)]
    private float m_SensitivityScale = 1;
    [SerializeField]
    private float m_TiltAngleMin = -10;
    [SerializeField]
    private float m_TiltAngleMax = 10;

    [SerializeField, MinMaxSlider(5, 179, true)]
    private Vector2 m_FieldOfViewAtZoom = new Vector2(30, 60);
    [SerializeField, MinMaxSlider(0, 2, true)]
    private Vector2 m_RotationSensitivityAtZoom = new Vector2(0.1f, 1);


    private ZoomFactorWidget m_ZoomFactorWidget;
    private ZoomViewWidget m_ZoomViewWidget;

    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsFilter m_FilterActive;

    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<LookAtViewComponent> m_PoolLookAtView;
    private EcsPool<ZoomViewComponent> m_PoolZoomView;
    private EcsPool<ZoomViewActiveEvent> m_PoolZoomAciitveEvent;

    private void OnChangeZoomFactor(float value)
    {
        m_ZoomFactor = value;
        m_ZoomViewWidget.SetZoomFactor(value);
    }



    private float m_ZoomFactor;

    public void SetDelta(Vector2 delta)
    {
        m_ZoomViewWidget.SetDeltaParalax(delta);
    }

    public void Init(IEcsSystems systems)
    {
        m_ZoomFactorWidget = UISystem.Instance.GetElement<ZoomFactorWidget>();
        m_ZoomFactorWidget.OnChangeZoomFactor += OnChangeZoomFactor;

        m_ZoomViewWidget = UISystem.Instance.GetElement<ZoomViewWidget>();

        m_World = systems.GetWorld();

        m_Filter = m_World.Filter<ZoomViewComponent>().Inc<PlayerTag>().End();
        m_FilterActive = m_World.Filter<ZoomViewComponent>().Inc<ZoomViewActiveEvent>().Inc<PlayerTag>().End();

        m_PoolEye = m_World.GetPool<EyeComponent>();
        m_PoolZoomView = m_World.GetPool<ZoomViewComponent>();
        m_PoolZoomAciitveEvent = m_World.GetPool<ZoomViewActiveEvent>();
        m_PoolLookAtView = m_World.GetPool<LookAtViewComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_FilterActive)
        {
            ref var zoomView = ref m_PoolZoomView.Get(entity);
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);
            zoomView.eyeEntity = lookAtView.eyeEntity;
            lookAtView.activeView = ViewType.Zoom;
            lookAtView.tiltAngleMin = m_TiltAngleMin;
            lookAtView.tiltAngleMax = m_TiltAngleMax;

            lookAtView.sensitivityVertical = m_SensitivityVertical;
            lookAtView.sensitivityHorizontal = m_SensitivityHorizontal;
            lookAtView.sencitivityScale = m_SensitivityScale;
        }

        foreach (var entity in m_Filter)
        {         
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);
            if (lookAtView.activeView != ViewType.Zoom) continue;

            ref var zoomView = ref m_PoolZoomView.Get(entity);

            zoomView.position = zoomView.transform.position + Vector3.up * zoomView.height;
            Vector3 direction = Vector3.Normalize(lookAtView.position - zoomView.position);
            zoomView.rotation = Quaternion.LookRotation(direction);

            lookAtView.sencitivityScale = m_SensitivityScale * Mathf.Lerp(m_RotationSensitivityAtZoom.y, m_RotationSensitivityAtZoom.x, m_ZoomFactor);

            if (zoomView.eyeEntity.Unpack(m_World, out int eyeEntity))
            {
                ref var eye = ref m_PoolEye.Get(eyeEntity);
                eye.rotation = zoomView.rotation;
                eye.position = zoomView.position;
                eye.fieldOfView = Mathf.Lerp(m_FieldOfViewAtZoom.y, m_FieldOfViewAtZoom.x, m_ZoomFactor);

            }
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        if (m_ZoomFactorWidget) m_ZoomFactorWidget.OnChangeZoomFactor -= OnChangeZoomFactor;
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_FilterActive)
        {
            m_PoolZoomAciitveEvent.Del(entity);
        }
    }
}
