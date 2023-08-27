using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

public class TurretAimConstrain : MonoBehaviour
{
    [System.Serializable]
    public class Constrain
    {
        [SerializeField]
        private Transform m_Parent;
        [SerializeField]
        private Transform m_Transform;
        [SerializeField]
        private Vector3 m_AxisRotate = Vector3.up;
        [SerializeField]
        private float m_AngleSpeed;
        [SerializeField]
        private bool m_IsClamp = false;
        [SerializeField, ShowIf("m_IsClamp")]
        private Vector3 m_ClampAxisDirection = Vector3.forward;
        [SerializeField, MinMaxSlider(-180, 180, true), ShowIf("m_IsClamp")]
        private Vector2 m_ClampAngle = new Vector2(-90, 90);
        [SerializeField]
        private float m_ReadyAngleThreshold = 3f;
        [SerializeField]
        private AimState m_AimState;

        private Quaternion m_IdleRotation;

        private bool m_IsInit;

        public void SetState(AimState state)
        {
            m_AimState = state;
        }

        public void Prepare()
        {
            m_IsInit = true;
            //m_Proxy.localPosition = m_Transform.localPosition;
            //m_Proxy.localRotation = m_Transform.localRotation;
            m_IdleRotation = m_Transform.localRotation;
        }

        public bool isReadyAngle => m_DeltaAngle < m_ReadyAngleThreshold;

        private float m_DeltaAngle;


        private Quaternion m_LocalRotation;


        public void Perform(Vector3 aimPoint)
        {
            if (!m_IsInit) Prepare();

            var direction = Vector3.Normalize(aimPoint - m_Transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion rotation = m_AimState == AimState.Idle ? m_IdleRotation : QuaternionUtility.ProjectOnPlane(targetRotation, m_Parent.rotation, m_AxisRotate);


            if (m_IsClamp)
            {
                //rotation
                if (m_AngleSpeed > 0)
                {
                    m_LocalRotation = rotation;
                    m_Transform.localRotation = QuaternionUtility.ClampAxisAngleSmooth(m_Transform.localRotation, m_LocalRotation, Quaternion.identity, m_ClampAngle.x, m_ClampAngle.y, m_AxisRotate, Time.deltaTime * m_AngleSpeed);
                }
                else
                {
                    m_LocalRotation = QuaternionUtility.ClampAxisAngle(rotation, Quaternion.identity, m_ClampAngle.x, m_ClampAngle.y, m_AxisRotate);
                    m_Transform.localRotation = rotation;
                }
            }
            else
            {
                if (m_AngleSpeed > 0)
                {
                    m_LocalRotation = rotation;
                    m_Transform.localRotation = QuaternionUtility.AxisAngleSmooth(m_Transform.localRotation, m_LocalRotation, Quaternion.identity, m_AxisRotate, Time.deltaTime * m_AngleSpeed);
                }
                else
                {
                    m_LocalRotation = rotation;
                    m_Transform.localRotation = rotation;
                }
            }

            m_DeltaAngle = Vector3.Angle(m_Transform.localRotation * Vector3.forward, m_LocalRotation * Vector3.forward);
        }

        public void DrawGizmos()
        {
#if UNITY_EDITOR
            if (m_Parent != null && m_Transform != null)
            {
                using (new Handles.DrawingScope(m_Parent.localToWorldMatrix))
                {

                    Vector2 clamp = m_IsClamp ? m_ClampAngle : new Vector2(-180, 180);

                    Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 0.2f);
                    Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
                    Handles.DrawSolidArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.x, 10);
                    Handles.DrawSolidArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.y, 10);

                    Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 1f);

                    Handles.DrawWireArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.x, 10, 2);
                    Handles.DrawWireArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.y, 10, 2);

                    Handles.zTest = UnityEngine.Rendering.CompareFunction.Greater;
                    Handles.color = new Color(m_AxisRotate.x, m_AxisRotate.y, m_AxisRotate.z, 0.05f);
                    Handles.DrawSolidArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.x, 10);
                    Handles.DrawSolidArc(m_Transform.localPosition, m_AxisRotate, Vector3.forward, -clamp.y, 10);
                }
            }
#endif
        }
    }
    [SerializeField]
    private Vector3 m_AimPoint;
    [SerializeField]
    private List<Constrain> m_ConstrainChain = new List<Constrain>();


    public void SetState(AimState aimState)
    {
        foreach (var chain in m_ConstrainChain)
        {
            chain.SetState(aimState);
        }
    }

    public void Perfrom(Vector3 aimPoint)
    {
        foreach (var chain in m_ConstrainChain)
        {
            chain.Perform(aimPoint);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var constrain in m_ConstrainChain)
        {
            constrain.DrawGizmos();
        }
    }
}
