using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;




public class InputCamera : MonoBehaviour
{
    //[SerializeField]
    //private Camera m_Camera;

    //[SerializeField, Range(0f, 1f)]
    //private float m_SensitivityHorizontal = 1;
    //[SerializeField, Range(0f, 1f)]
    //private float m_SensitivityVertical = 1;
    //[SerializeField, Range(0, 10)]
    //private float m_SensitivityScale = 1;

    //private CameraInputWidget m_InputWidget;

    ////private IInputCameraAxisListener m_AxisListener;

    //private Vector2 axis;

    //public Vector2 GetAxis()
    //{
    //    return Vector2.Scale(axis, new Vector2(m_SensitivityHorizontal, m_SensitivityVertical)) * m_SensitivityScale * Time.deltaTime;
    //}

    //private void OnBeginInput(Vector2 delta)
    //{
    //    axis = Vector2.zero;
    //}

    //private void OnUpdateInput(Vector2 delta)
    //{
    //    axis = delta;
    //}

    //private void OnEndInput(Vector2 delta)
    //{
    //    axis = Vector2.zero;
    //}

    ////public void SetListener(IInputCameraAxisListener listener)
    ////{
    ////    m_AxisListener = listener;
    ////}

    //private EcsFilter m_Filter;
    //private EcsPool<ViewComponent> m_PoolViewComponent;

    //public void Init(IEcsSystems systems)
    //{
    //    var world = systems.GetWorld();
    //    m_Filter = world.Filter<ViewComponent>().Inc<PlayerTag>().End();
    //    m_PoolViewComponent = world.GetPool<ViewComponent>();

    //    m_InputWidget = UISystem.Instance.GetElement<CameraInputWidget>();
    //    m_InputWidget.OnBeginInput += OnBeginInput;
    //    m_InputWidget.OnUpdateInput += OnUpdateInput;
    //    m_InputWidget.OnEndInput += OnEndInput;
    //}

    //public void Run(IEcsSystems systems)
    //{
    //    foreach (var entity in m_Filter)
    //    {
    //        var viewComponent = m_PoolViewComponent.Get(entity);
    //        if (!viewComponent.viewController.eyeView.HasView())
    //        {
    //            viewComponent.viewController.eyeView.SetCamera(m_Camera);
    //            viewComponent.viewController.eyeView.SetView(viewComponent.viewController.GetView<ShipOrbitView>(), 0);
    //        }
    //        viewComponent.viewController.AddInputDelta(GetAxis());
    //        viewComponent.viewController.RunUpdate();
    //    }
    //}

    //public void Destroy(IEcsSystems systems)
    //{
    //    if (m_InputWidget != null)
    //    {
    //        m_InputWidget.OnBeginInput -= OnBeginInput;
    //        m_InputWidget.OnUpdateInput -= OnUpdateInput;
    //        m_InputWidget.OnEndInput -= OnEndInput;
    //    }
    //}
}
