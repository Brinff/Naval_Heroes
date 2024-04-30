using Game.UI;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class PlayerViewSwitchSystem : MonoBehaviour, IEcsInitSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    private ZoomToggleWidget m_ZoomToggleWidget;
    private EcsWorld m_World;
    private EcsFilter m_Filter;

    //private ViewType m_ViewType;
    //private EcsPool<LookAtViewComponent> m_PoolLookAtView;
    //private ViewType m_CheckViewType;

    [Button]
    public void Zoom()
    {
        ServiceLocator.Get<UIController>().compositionModule.Show<UIGameShipZoomComposition>();
        var entity = m_Filter.GetSingleton();
        if (entity != null) m_World.GetPool<ZoomViewActiveEvent>().Add(entity.Value);
    }

    [Button]
    public void Orbit()
    {
        ServiceLocator.Get<UIController>().compositionModule.Show<UIGameShipDefaultComposition>();
        var entity = m_Filter.GetSingleton();
        if (entity != null) m_World.GetPool<OrbitViewActiveEvent>().Add(entity.Value);
    }

    [Button]
    public void Home()
    {
        var playerEye = m_World.Filter<PlayerTag>().Inc<EyeComponent>().End().GetSingleton();
        if (playerEye != null)
        {

        }
        //m_ViewType = ViewType.Home;
    }

    private void OnToggleZoom(bool value)
    {
        if (value) Zoom();
        else Orbit();
        m_ZoomToggleWidget.SetToggle(value);
    }

    public void Destroy(IEcsSystems systems)
    {
        if (m_ZoomToggleWidget) m_ZoomToggleWidget.OnToggle -= OnToggleZoom;
    }

    public void Init(IEcsSystems systems)
    {
        m_ZoomToggleWidget = ServiceLocator.Get<UIController>().GetElement<ZoomToggleWidget>();
        m_ZoomToggleWidget.OnToggle += OnToggleZoom;

        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTag>().Inc<CommanderTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        //foreach (var entity in m_Filter)
        //{
        //    if (m_ViewType != m_CheckViewType)
        //    {
        //        ref var lookAtView = ref m_PoolLookAtView.Get(entity);

        //        if (m_ViewType == ViewType.Zoom) m_World.GetPool<ZoomViewActiveEvent>().Add(entity);
        //        if (m_ViewType == ViewType.Orbit) m_World.GetPool<OrbitViewActiveEvent>().Add(entity);
        //        if (m_ViewType == ViewType.Home) m_World.GetPool<HomeViewActiveEvent>().Add(entity);
        //        m_CheckViewType = m_ViewType;

        //    }
        //}
    }
}
