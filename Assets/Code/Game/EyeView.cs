using DG.Tweening;
using DG.Tweening.Core;

using UnityEngine;
using Sirenix.OdinInspector;
using System;

public static class ViewExtention
{
    public static T As<T>(this IView view)
    {
        if (view is T) return (T)view;
        return default(T);
    }
}

public interface IView
{

}

public interface IViewPerform : IView
{
    void Perform(float deltaTime);
}

public interface IViewTarget : IView
{
    void SetTarget(Transform target);
}

public interface IViewPosition : IView
{
    Vector3 position { get; }
}

public interface IViewRotation : IView
{
    Quaternion rotation { get; }
}

public interface IViewFieldOfView : IView
{
    float fieldOfView { get; }
}

public interface IViewRotationSensitivity
{
    Vector2 rotationSensitivity { get; }
}

public class EyeView : SerializedMonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;
    [SerializeField, Range(5,179)]
    private float m_FieldOfView = 60;
    [SerializeField]
    private Vector2 m_RotationSensitivity = new Vector2(1, 1);

    [ShowInInspector]
    private IViewPosition m_ViewPosition;
    [ShowInInspector]
    private IViewRotation m_ViewRotation;
    [ShowInInspector]
    private IViewFieldOfView m_ViewFieldOfView;
    [ShowInInspector]
    private IViewRotationSensitivity m_ViewRotationSensitivity;

    public float fieldOfView
    {
        get
        {
            return m_FieldOfView;
        }

        private set 
        {
            m_FieldOfView = value;
            if (m_Camera) m_Camera.fieldOfView = value;
        }
    }

    public Vector2 rotationSensitivity
    {
        get
        {
            return m_RotationSensitivity;
        }
        private set
        {
            m_RotationSensitivity = value;
        }
    }

    public void SetRotationSensitivity(Vector2 rotationSensitivity)
    {
        m_RotationSensitivity = rotationSensitivity;
    }

    public void SetViewFieldOfView(IViewFieldOfView viewFieldOfView)
    {
        m_ViewFieldOfView = viewFieldOfView;
    }

    public void SetViewRotation(IViewRotation viewRotation)
    {
        m_ViewRotation = viewRotation;
    }

    public void SetViewRotationSensitivity(IViewRotationSensitivity viewRotationSensitivity)
    {
        m_ViewRotationSensitivity = viewRotationSensitivity;
    }

    public void SetViewPosition(IViewPosition viewPosition)
    {
        m_ViewPosition = viewPosition;
    }

    public void SetCamera(Camera camera)
    {
        m_Camera = camera;
        if (m_Camera)
        {
            m_Camera.transform.SetParent(transform);
            m_Camera.transform.ResetAll();
        }
    }

    public bool HasView()
    {
        return m_View != null;
    }

    public bool ContainsView(IView view)
    {
        return m_View == view;
    }

    private Tween m_ViewTween;
    private float m_ViewTweenFactor;

    private Vector2 m_FromRotationSensitivity;
    private float m_FromFieldOfView;
    private Vector3 m_FromPosition;
    private Quaternion m_FromRotation;

    private IView m_View;

    public Tween SetView(IView view, float duration)
    {
        m_View = view;
        if (m_ViewTween != null) m_ViewTween.Kill();

        if (Mathf.Approximately(duration, 0))
        {
            SetViewPosition(view.As<IViewPosition>());
            SetViewRotation(view.As<IViewRotation>());
            SetViewFieldOfView(view.As<IViewFieldOfView>());
            SetViewRotationSensitivity(view.As<IViewRotationSensitivity>());
            UpdateView();
            return null;
        }

        m_ViewRotation = null;
        m_ViewPosition = null;
        m_ViewFieldOfView = null;
        m_ViewRotationSensitivity = null;


        m_ViewTweenFactor = 0;

        m_FromPosition = transform.position;
        m_FromRotation = transform.rotation;
        m_FromFieldOfView = m_FieldOfView;
        m_FromRotationSensitivity = m_RotationSensitivity;

        DOGetter<float> tweenGeter = () => m_ViewTweenFactor;
        DOSetter<float> tweenSeter = (float factor) =>
        {
            m_ViewTweenFactor = factor;
            transform.position = Vector3.Lerp(m_FromPosition, view.As<IViewPosition>().position, factor);
            transform.rotation = Quaternion.Lerp(m_FromRotation, view.As<IViewRotation>().rotation, factor);
            fieldOfView = Mathf.Lerp(m_FromFieldOfView, view.As<IViewFieldOfView>().fieldOfView, factor);
            rotationSensitivity = Vector2.Lerp(m_RotationSensitivity, view.As<IViewRotationSensitivity>().rotationSensitivity, factor);

        };

        TweenCallback complete = () =>
        {
            SetViewPosition(view.As<IViewPosition>());
            SetViewRotation(view.As<IViewRotation>());
            SetViewFieldOfView(view.As<IViewFieldOfView>());
            SetViewRotationSensitivity(view.As<IViewRotationSensitivity>());
        };

        m_ViewTween = DOTween.To(tweenGeter, tweenSeter, 1, duration).OnComplete(complete);

        return m_ViewTween;
    }

    private void UpdateView()
    {
        if (m_ViewPosition != null) transform.position = m_ViewPosition.position;
        if (m_ViewRotation != null) transform.rotation = m_ViewRotation.rotation;
        if (m_ViewFieldOfView != null) fieldOfView = m_ViewFieldOfView.fieldOfView;
        if (m_ViewRotationSensitivity != null) rotationSensitivity = m_ViewRotationSensitivity.rotationSensitivity;
    }

    private void LateUpdate()
    {
        UpdateView();
    }

    public Ray GetRay()
    {
        return new Ray(transform.position, transform.forward);
    }

    public bool HasCamera()
    {
        return m_Camera;
    }
}
