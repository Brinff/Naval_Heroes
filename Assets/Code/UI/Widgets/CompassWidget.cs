using Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CompassWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private RectTransform m_DirectionsView;
    [SerializeField]
    private RectTransform m_EnemyView;

    [SerializeField]
    private RectTransform m_ForwardIndicator;
    [SerializeField]
    private CompasElement[] m_Chars;
    [SerializeField]
    private CompasElement[] m_Line;


    [System.Serializable]
    public class CompasElement
    {
        public RectTransform rectTransform;
        public float angle;
    }

    [SerializeField]
    private CompassEnemyIndicator m_EnemyIndicatorPrefab;

    [SerializeField, Range(5, 360)]
    private float m_AngleView = 180;

    private List<GameObject> m_Items = new List<GameObject>();


    public void Clear()
    {
        foreach (var item in m_Items)
        {
            Destroy(item);
        }

        m_Items.Clear();
    }

    public void Hide(CompassEnemyIndicator compassEnemyIndicator)
    {
        m_Items.Remove(compassEnemyIndicator.gameObject);
        Destroy(compassEnemyIndicator.gameObject);
    }

    public CompassEnemyIndicator CreateEnemyIndicator(Vector3 position)
    {
        var indicator = Instantiate(m_EnemyIndicatorPrefab);
        indicator.transform.SetParent(m_EnemyView);
        indicator.rectTransform.anchoredPosition = m_EnemyIndicatorPrefab.rectTransform.anchoredPosition;
        indicator.gameObject.SetActive(true);
        m_Items.Add(indicator.gameObject);
        UpdateIndicator(indicator.rectTransform, position);
        return indicator;
    }

    private float GetOffset(Vector3 direction, bool isClamp)
    {
        direction = Quaternion.Inverse(m_Rotation) * direction;
        float angle = Mathf.Atan2(direction.x, direction.z);
        if (isClamp)
        {
            float clampAngle = m_AngleView * Mathf.Deg2Rad / 2;
            angle = Mathf.Clamp(angle, -clampAngle, clampAngle);
        }
        return GetOffset(angle);
    }

    [SerializeField]
    private Quaternion m_Rotation = Quaternion.identity;
    [SerializeField]
    private Quaternion m_Forward = Quaternion.identity;
    [SerializeField]
    private Vector3 m_Position = Vector3.zero;

    public void SetForward(Quaternion forward)
    {
        m_Forward = forward;
    }

    public void SetRotation(Quaternion rotation)
    {
        m_Rotation = rotation;
    }

    public void SetPosition(Vector3 position)
    {
        m_Position = position;
    }

    public void UpdateIndicator(RectTransform indicator, Vector3 position)
    {
        Vector3 direction = Vector3.Normalize(position - m_Position);
        indicator.anchoredPosition = new Vector2(GetOffset(direction, true), indicator.anchoredPosition.y);
    }

    private void Update()
    {
        m_ForwardIndicator.anchoredPosition = new Vector2(GetOffset(m_Forward * Vector3.forward, true), m_ForwardIndicator.anchoredPosition.y);

        for (int i = 0; i < m_Chars.Length; i++)
        {
            var directionChar = m_Chars[i];
            if (directionChar.rectTransform)
            {
                Vector3 direction = Quaternion.Inverse(m_Rotation) * Vector3.forward;
                float angle = Mathf.Atan2(direction.x, direction.z);
                directionChar.rectTransform.anchoredPosition = Vector3.right * GetOffset(angle + directionChar.angle * Mathf.Deg2Rad);
                //directionChar.rectTransform.sizeDelta = new Vector2(GetSize(90 * Mathf.Deg2Rad), m_DirectionsView.rect.height);
            }
        }

        for (int i = 0; i < m_Line.Length; i++)
        {
            var line = m_Line[i];
            if (line.rectTransform)
            {
                Vector3 direction = Quaternion.Inverse(m_Rotation) * Vector3.forward;
                float angle = Mathf.Atan2(direction.x, direction.z);
                line.rectTransform.anchoredPosition = Vector3.right * GetOffset(angle + line.angle * Mathf.Deg2Rad);
                line.rectTransform.sizeDelta = new Vector2(GetSize(90 * Mathf.Deg2Rad), m_DirectionsView.rect.height);
            }
        }
    }

    private float GetSize(float angle)
    {
        return angle / (m_AngleView * Mathf.Deg2Rad) * (float)m_DirectionsView.rect.size.x;
    }

    private float GetOffset(float angle)
    {
        return angle / (m_AngleView * Mathf.Deg2Rad) * (float)m_DirectionsView.rect.size.x;
    }

    public void Show(bool immediately)
    {
        m_EnemyIndicatorPrefab.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }
}
