using DG.Tweening;
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

    public GameObject CreatePlane(Vector3 worldPosition)
    {
       return  Create(m_PlanePrefab, worldPosition);
    }

    public GameObject CreateTorpedo(Vector3 worldPosition)
    {
       return Create(m_TorpedoPrefab, worldPosition);
    }

    public void Create(GameObject prefab, Vector3 worldPosition, float time)
    {
        var instance = Create(prefab, worldPosition);
        Destroy(instance, time);
    }

    public void Hide(GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.one * 0.8f;
        gameObject.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);

        var group = gameObject.GetComponent<CanvasGroup>();
        group.DOFade(0, 0.3f).OnComplete(() => { Destroy(group.gameObject); });
    }

    public GameObject Create(GameObject prefab, Vector3 worldPosition)
    {
        var instance = Instantiate(prefab);

        var group = instance.GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.DOFade(1, 0.1f);

        instance.transform.SetParent(m_View);
        instance.transform.localScale = Vector3.one * 0.8f;
        instance.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);

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
