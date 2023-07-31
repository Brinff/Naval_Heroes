using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretDirectionConstrain : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_AxisRotate = Vector3.up;
    [SerializeField]
    private bool m_IsClamp = false;
    [SerializeField, ShowIf("m_IsClamp")]
    private Vector3 m_ClampAxisDirection = Vector3.forward;
    [SerializeField, MinMaxSlider(-180, 180, true), ShowIf("m_IsClamp")]
    private Vector2 m_ClampAngle = new Vector2(-90, 90);
    [SerializeField]
    private float m_AngleSpeed;

    [SerializeField]
    private Vector3 m_AimPoint;
    [SerializeField]
    private Transform m_Proxy;
    [SerializeField]
    private float m_ReadyAngleThreshold = 3f;
    [SerializeField]
    private AimState m_AimState;

    public void SetState(AimState state)
    {
        m_AimState = state;
    }

    [SerializeField]
    private Quaternion m_IdleRotation;

    private void OnEnable()
    {
        m_Proxy.localPosition = transform.localPosition;
        m_Proxy.localRotation = transform.localRotation;
        m_IdleRotation = transform.localRotation;
    }

    public void SetAimPoint(Vector3 aimPoint)
    {
        m_AimPoint = aimPoint;
    }


    public bool isReadyAngle => m_DeltaAngle < m_ReadyAngleThreshold;

    private float m_DeltaAngle;

    public void Perform()
    {
        var direction = Vector3.Normalize(m_AimPoint - transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        //smoothTargetRotation = Quaternion.Slerp(smoothTargetRotation, targetRotation, Time.deltaTime * 50);

        Quaternion rotation = m_AimState == AimState.Idle ? m_IdleRotation : QuaternionUtility.ProjectOnPlane(targetRotation, transform.parent.rotation, m_AxisRotate);


        if (m_IsClamp)
        {
            //rotation
            if (m_AngleSpeed > 0)
            {
                m_Proxy.localRotation = rotation;
                transform.localRotation = QuaternionUtility.ClampAxisAngleSmooth(transform.localRotation, m_Proxy.localRotation, Quaternion.identity, m_ClampAngle.x, m_ClampAngle.y, m_AxisRotate, Time.deltaTime * m_AngleSpeed);
            }
            else
            {
                m_Proxy.localRotation =  QuaternionUtility.ClampAxisAngle(rotation, Quaternion.identity, m_ClampAngle.x, m_ClampAngle.y, m_AxisRotate);
                transform.localRotation = rotation;
            }
        }
        else
        {
            if (m_AngleSpeed > 0)
            { 
                m_Proxy.localRotation = rotation;
                transform.localRotation = QuaternionUtility.AxisAngleSmooth(transform.localRotation, m_Proxy.localRotation, Quaternion.identity, m_AxisRotate, Time.deltaTime * m_AngleSpeed);
            }
            else
            {
                m_Proxy.localRotation = rotation;
                transform.localRotation = rotation;
            }
        }

        m_DeltaAngle = Vector3.Angle(transform.localRotation * Vector3.forward, m_Proxy.localRotation * Vector3.forward);
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        using (new Handles.DrawingScope(transform.parent.localToWorldMatrix))
        {

            Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 0.2f);
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            Handles.DrawSolidArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.x, 10);
            Handles.DrawSolidArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.y, 10);

            Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 1f);

            Handles.DrawWireArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.x, 10, 2);
            Handles.DrawWireArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.y, 10, 2);

            Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
            Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 0.05f);
            Handles.DrawSolidArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.x, 10);
            Handles.DrawSolidArc(transform.localPosition, m_AxisRotate, Vector3.forward, -m_ClampAngle.y, 10);


        }
    }

#endif
}
