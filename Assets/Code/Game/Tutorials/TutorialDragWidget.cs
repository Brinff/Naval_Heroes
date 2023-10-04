using DG.Tweening;
using Game.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDragWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private RectTransform m_View;
    [SerializeField]
    private RectTransform m_Pointer;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private CanvasGroup m_Group;

    private int m_BeginDragPointerID = Animator.StringToHash("Begin Drag Pointer");
    private int m_EndDragPointerID = Animator.StringToHash("End Drag Pointer");


    private bool m_IsShowed;

    [Button]
    public void Hide(bool immediately)
    {
        m_IsShowed = false;
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

        if (m_Coroutine != null) StopCoroutine(m_Coroutine);
    }

    [Button]
    public void Show(bool immediately)
    {
        m_IsShowed = true;
        gameObject.SetActive(true);

        if (immediately)
        {
            m_Group.alpha = 1;
        }
        else
        {
            m_Group.DOFade(1, 0.2f);
        }

        if (m_Coroutine != null) StopCoroutine(m_Coroutine);
        m_Coroutine = StartCoroutine(Move());
    }

    private Vector2 m_BeginLocalPoint;
    private Vector2 m_EndLocalPoint;
    private Coroutine m_Coroutine;
    [Button]
    public void PlaceAtScreen(Vector2 beginScreenPoint, Vector2 endScreenPoint)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, beginScreenPoint, null, out m_BeginLocalPoint) && 
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_View, endScreenPoint, null, out m_EndLocalPoint))
        {
            if (m_IsShowed)
            {
                if (m_Coroutine != null) StopCoroutine(m_Coroutine);
                m_Coroutine = StartCoroutine(Move());
            }
        }
    }

    private IEnumerator Move()
    {
        while (m_IsShowed)
        {
            m_Pointer.localPosition = m_BeginLocalPoint;
            m_Animator.Play(m_BeginDragPointerID, 0, 0);
            yield return new WaitForSeconds(0.7f);
            yield return m_Pointer.DOLocalMove(m_EndLocalPoint, 1f).WaitForCompletion();
            yield return new WaitForSeconds(0.2f);
            m_Animator.Play(m_EndDragPointerID, 0, 0);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PlaceAtWorld(Vector3 beginWorldPoint, Vector3 endWorldPoint)
    {
        var beginScreenPoint = Camera.main.WorldToScreenPoint(beginWorldPoint);
        var endScreenPoint = Camera.main.WorldToScreenPoint(endWorldPoint);
        PlaceAtScreen(beginScreenPoint, endScreenPoint);
    }
}
