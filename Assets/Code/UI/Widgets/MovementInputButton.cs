using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Flags]
    public enum Direction { None = 0, Left = 1, Right = 2, Forward = 4, Backward = 8 }
    [SerializeField]
    private Direction m_Direction;
    private MovementInputButtonsWidget buttonsWidget;

    [SerializeField]
    private Image m_FillImage;

    private void OnEnable()
    {
        buttonsWidget = GetComponentInParent<MovementInputButtonsWidget>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_FillImage)
        {
            m_FillImage.DOKill();
            m_FillImage.DOFade(1, 0.1f);
        }
        transform.DOKill();
        transform.DOScale(0.9f, 0.1f);
        buttonsWidget?.OnButtonDown(m_Direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_FillImage)
        {
            m_FillImage.DOKill();
            m_FillImage.DOFade(0, 0.1f);
        }
        transform.DOKill();
        transform.DOScale(1f, 0.1f);
        buttonsWidget?.OnButtonUp(m_Direction);
    }
}
