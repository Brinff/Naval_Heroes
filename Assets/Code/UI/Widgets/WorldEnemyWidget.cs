using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using System;
using UnityEngine.Android;

public class WorldEnemyWidget : MonoBehaviour, IUIElement
{

    private Camera m_WorldCamera;

    [SerializeField]
    private GameObject m_Prefab;

    public void SetWorldCamera(Camera camera)
    {
        m_WorldCamera = camera;
    }

    public void UpdatePosition(RectTransform rectTransform, Vector3 position)
    {
        float dot = Vector3.Dot(m_WorldCamera.transform.forward, Vector3.Normalize(position - m_WorldCamera.transform.position));
        rectTransform.gameObject.SetActive(dot > 0);
        rectTransform.anchoredPosition = m_WorldCamera.WorldToScreenPoint(position);
    }

    public GameObject Register(Transform target)
    {
        var element = Instantiate(m_Prefab);
        var rectTransform = element.GetComponent<RectTransform>();

        rectTransform.SetParent(transform);

        rectTransform.anchoredPosition = m_WorldCamera.WorldToScreenPoint(target.position);
        element.SetActive(true);

        return element;
    }

    public void Unregister(GameObject gameObject)
    {
        Destroy(gameObject, 1);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }
}
