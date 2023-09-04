using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotItemInfoWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private SlotItemInfoItem m_Prefab;
    [SerializeField]
    private RectTransform m_View;

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        m_Prefab.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public SlotItemInfoItem Create()
    {
        var instance = Instantiate(m_Prefab);
        instance.transform.SetParent(transform, false);
        instance.gameObject.SetActive(true);
        instance.Prepare(m_WorldCamera = Camera.main, m_View);
        return instance;
    }

    private Camera m_WorldCamera;


    
    public void UpdatePosition(RectTransform rectTransform, Vector3 position)
    {
        if (m_WorldCamera == null) m_WorldCamera = Camera.main;

        float dot = Vector3.Dot(m_WorldCamera.transform.forward, Vector3.Normalize(position - m_WorldCamera.transform.position));
        rectTransform.gameObject.SetActive(dot > 0);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, m_WorldCamera.WorldToScreenPoint(position), null, out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }
}
