using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FireInputWidget : MonoBehaviour, IUIElement, IPointerDownHandler, IPointerUpHandler
{
    public delegate void FireInputDelegate(bool isFire);

    public event FireInputDelegate OnPerform;
    [SerializeField]
    private Image m_FillImage;

    public void Hide(bool immediately)
    {
        OnPerform?.Invoke(false);
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(0.9f, 0.1f);

        m_FillImage.DOKill();
        m_FillImage.DOFade(1, 0.1f);

        OnPerform?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, 0.1f);
        m_FillImage.DOKill();
        m_FillImage.DOFade(0, 0.1f);
        OnPerform?.Invoke(false);
    }
}
