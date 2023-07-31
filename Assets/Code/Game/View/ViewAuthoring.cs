using Leopotam.EcsLite;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering.Universal;

public struct ViewComponent
{
    public float aimingDistance;
    public ViewAuthoring viewController;
}

public class ViewAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    [SerializeField]
    private EyeView m_EyeView;

    public EyeView eyeView => m_EyeView;
    [SerializeField]
    private Transform m_InfoView;

    public Transform infoView => m_InfoView;

    [SerializeField]
    private Transform m_Target;
    [SerializeField]
    private float m_AimingDistance = 500;

    private List<IView> m_Views = new List<IView>();




    private void OnEnable()
    {
        GetComponentsInChildren<IView>(m_Views);
        foreach (var view in m_Views)
        {
            view.As<IViewTarget>().SetTarget(m_Target);
        }
    }

    public T GetView<T>() where T : IView
    {
        var view = m_Views.Find(x => x is T);
        if (view == null) return default(T);
        return view.As<T>();
    }

    public enum TypeRotation { Global, Local }
    [SerializeField]
    public TypeRotation m_RotationSpace = TypeRotation.Global;

    [SerializeField]
    private Vector3 m_GlobalRotation;
    [SerializeField]
    private Vector3 m_LocalRotation;
    [SerializeField]
    private float m_MinVerticalAngle = -20;
    [SerializeField]
    private float m_MaxVerticalAngle = 20;

    [SerializeField]
    private Transform m_ViewOrgin;

    private Ray m_Ray;

    public void RunUpdate()
    {
        if (m_RotationSpace == TypeRotation.Global)
        {
            Quaternion rotation = Quaternion.Euler(m_GlobalRotation);
            //m_TargetView.position = transform.position +  * m_AimingDistance;
            m_Ray.origin = m_ViewOrgin.position;
            m_Ray.direction = rotation * Vector3.forward;
            m_LocalRotation = (Quaternion.Inverse(transform.rotation) * rotation).eulerAngles;
        }

        if (m_RotationSpace == TypeRotation.Local)
        {
            Quaternion rotation = Quaternion.Euler(m_LocalRotation);

            m_Ray.origin = m_ViewOrgin.position;
            m_Ray.direction = Quaternion.Euler(m_LocalRotation) * Vector3.forward * m_AimingDistance;
            m_GlobalRotation = (transform.rotation * rotation).eulerAngles;
        }

        //Plane plane = new Plane(Vector3.up, 0);
        //float distance = m_AimingDistance;
        //if(plane.Raycast(m_Ray, out float d))
        //{
        //    distance = d;
        //}

        m_Target.position = m_Ray.GetPoint(m_AimingDistance);

        foreach (var view in m_Views)
        {
            view.As<IViewPerform>().Perform(Time.deltaTime);
        }
    }

    public Vector2 AddInputDelta(Vector2 delta)
    {
        if (m_EyeView != null) delta *= m_EyeView.rotationSensitivity;

        Vector3 angle = new Vector3(-delta.y, delta.x, 0);
        if (m_RotationSpace == TypeRotation.Global)
        {
            m_GlobalRotation = ConstrainAngles(m_GlobalRotation + angle);
        }
        if (m_RotationSpace == TypeRotation.Local)
        {
            m_LocalRotation = ConstrainAngles(m_LocalRotation + angle);
        }
        return delta;
    }

    public Vector3 ConstrainAngles(Vector3 angle)
    {
        angle.x = Mathf.Clamp(angle.x, -m_MaxVerticalAngle, -m_MinVerticalAngle);

        //if (angle.x > 0 && angle.x < 360 + m_MinVerticalAngle)
        //{
        //    if (angle.x > m_MaxVerticalAngle) angle.x = m_MaxVerticalAngle;
        //}

        //if (angle.x > 0 && angle.x > 360 + m_MinVerticalAngle)
        //{
        //    if (angle.x > 360 + m_MinVerticalAngle) angle.x = 360 - m_MinVerticalAngle;
        //}

        //Debug.Log(angle);
        if (angle.y < 0f)
        {
            angle.y += 360f;
        }
        else if (angle.y >= 360f)
        {
            angle.y -= 360f;
        }
        return angle;
    }

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var viewComponent = ref ecsWorld.GetPool<ViewComponent>().Add(entity);
        viewComponent.viewController = this;
        viewComponent.aimingDistance = m_AimingDistance;
    }
}
