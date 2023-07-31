using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShipZoomView : MonoBehaviour, IViewPosition, IViewRotation, IViewTarget, IViewPerform, IViewFieldOfView, IViewRotationSensitivity
{
    [SerializeField]
    private float m_Height;
    [SerializeField, MinMaxSlider(5, 179, true)]
    private Vector2 m_FieldOfViewAtZoom = new Vector2(30, 60);
    [SerializeField, MinMaxSlider(0, 2, true)]
    private Vector2 m_RotationSensitivityAtZoom = new Vector2(0.1f, 1);
    [SerializeField]
    private Vector2 m_RotationSensitivity = new Vector2(1, 1);

    [SerializeField, Range(0, 1)]
    private float m_ZoomFactor;

    private Vector3 m_Position;

    private Quaternion m_Rotation;

    private Transform m_Target;

    public Vector3 position => m_Position;
    public Quaternion rotation => m_Rotation;

    public float fieldOfView => Mathf.Lerp(m_FieldOfViewAtZoom.y, m_FieldOfViewAtZoom.x, m_ZoomFactor);

    public Vector2 rotationSensitivity => m_RotationSensitivity * Mathf.Lerp(m_RotationSensitivityAtZoom.y, m_RotationSensitivityAtZoom.x, m_ZoomFactor);

    public void Perform(float deltaTime)
    {
        if (m_Target != null)
        {
            m_Position = transform.position + Vector3.up * m_Height;
            Vector3 direction = Vector3.Normalize(m_Target.position - m_Position);
            m_Rotation = Quaternion.LookRotation(direction);
        }
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    public void SetZoomFactor(float zoomFactor)
    {
        m_ZoomFactor = zoomFactor;
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position + Vector3.up * m_Height;
        Gizmos.DrawSphere(position, 1);
    }
}
