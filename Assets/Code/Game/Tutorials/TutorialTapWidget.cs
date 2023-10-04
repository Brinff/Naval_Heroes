using DG.Tweening;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTapWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private RectTransform m_View;
    [SerializeField]
    private RectTransform m_Pointer;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private CanvasGroup m_Group;

    private int m_TapPointerID = Animator.StringToHash("Tap Pointer");

    public void Hide(bool immediately)
    {
        if (immediately)
        {
            m_Group.alpha = 0;
            gameObject.SetActive(false);
        }
        else
        {
            m_Group.alpha = 0;
            gameObject.SetActive(false);
        }
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
        m_Animator.Play(m_TapPointerID, 0, 0);

        if (immediately)
        {
            m_Group.alpha = 1;
        }
        else
        {
            m_Group.DOFade(1, 0.2f);
        }
    }

    public void PlaceAtScreen(Vector2 screenPoint)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, screenPoint, null, out Vector2 localPoint))
        {
            m_Pointer.localPosition = localPoint;
        }
    }

    public void PlaceAtWorld(Vector3 worldPoint)
    {
        var screenPoint = Camera.main.WorldToScreenPoint(worldPoint);
        PlaceAtScreen(screenPoint);
    }
}
