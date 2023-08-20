using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomeViewSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsFilter m_FilterActive;

    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<HomeViewComponent> m_PoolHomeView;
    private EcsPool<HomeViewActiveEvent> m_PoolHomeViewActiveEvent;
    private EcsPool<LookAtViewComponent> m_PoolLookAtView;

    public void Init(IEcsSystems systems)
    {
        //m_World = systems.GetWorld();

        //m_Filter = m_World.Filter<HomeViewComponent>().End();
        //m_FilterActive = m_World.Filter<HomeViewComponent>().Inc<HomeViewActiveEvent>().End();

        //m_PoolEye = m_World.GetPool<EyeComponent>();
        //m_PoolHomeView = m_World.GetPool<HomeViewComponent>();
        //m_PoolHomeViewActiveEvent = m_World.GetPool<HomeViewActiveEvent>();
        //m_PoolLookAtView = m_World.GetPool<LookAtViewComponent>();
    }

    public void PostRun(IEcsSystems systems)
    {
        //foreach (var entity in m_FilterActive)
        //{
        //    m_PoolHomeViewActiveEvent.Del(entity);
        //}
    }

    public void Run(IEcsSystems systems)
    {
        //foreach (var entity in m_FilterActive)
        //{
        //    ref var homeView = ref m_PoolHomeView.Get(entity);
        //    ref var lookAtView = ref m_PoolLookAtView.Get(entity);
        //    homeView.eyeEntity = lookAtView.eyeEntity;

        //    lookAtView.activeView = ViewType.Home;

        //    //lookAtView.tiltAngleMin = m_TiltAngleMin;
        //    //lookAtView.tiltAngleMax = m_TiltAngleMax;

        //    //lookAtView.sensitivityVertical = m_SensitivityVertical;
        //    //lookAtView.sensitivityHorizontal = m_SensitivityHorizontal;
        //    //lookAtView.sencitivityScale = m_SensitivityScale;
        //}

        //foreach (var entity in m_Filter)
        //{
        //    ref var lookAtView = ref m_PoolLookAtView.Get(entity);
        //    if (lookAtView.activeView != ViewType.Home) continue;

        //    ref var homeView = ref m_PoolHomeView.Get(entity);

        //    //zoomView.position = zoomView.transform.position + Vector3.up * zoomView.height;
        //    //Vector3 direction = Vector3.Normalize(lookAtView.position - zoomView.position);
        //    //zoomView.rotation = Quaternion.LookRotation(direction);

        //    //lookAtView.sencitivityScale = m_SensitivityScale * Mathf.Lerp(m_RotationSensitivityAtZoom.y, m_RotationSensitivityAtZoom.x, m_ZoomFactor);

        //    if (homeView.eyeEntity.Unpack(m_World, out int eyeEntity))
        //    {
        //        ref var eye = ref m_PoolEye.Get(eyeEntity);
        //        eye.rotation = homeView.orgin.rotation;
        //        eye.position = homeView.orgin.position;
        //        eye.fieldOfView = 70; //Mathf.Lerp(m_FieldOfViewAtZoom.y, m_FieldOfViewAtZoom.x, m_ZoomFactor);

        //    }
        //}
    }

}
