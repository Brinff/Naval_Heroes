using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetWidget : MonoBehaviour, IUIElement
{
    
    [SerializeField]
    private GameObject m_TorpedoPrefab;
    [SerializeField]
    private GameObject m_PlanePrefab;
    [SerializeField]
    private RectTransform m_View;


    private Camera m_WorldCamera;
    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void Create(GameObject prefab, Vector3 worldPosition, float time)
    {
        var instance = Instantiate(prefab);

        UpdatePosition(instance.transform as RectTransform, worldPosition);

        Destroy(instance, time);
    }

    public GameObject Create(GameObject prefab, Vector3 worldPosition)
    {
        var instance = Instantiate(prefab);

        UpdatePosition(instance.transform as RectTransform, worldPosition);
        return instance;
    }

    public void SetWorldCamera(Camera camera)
    {
        m_WorldCamera = camera;
    }

    public void UpdatePosition(RectTransform rectTransform, Vector3 position)
    {
        float dot = Vector3.Dot(m_WorldCamera.transform.forward, Vector3.Normalize(position - m_WorldCamera.transform.position));
        rectTransform.gameObject.SetActive(dot > 0);
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, m_WorldCamera.WorldToScreenPoint(position), null, out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint;
        }
    }

}
