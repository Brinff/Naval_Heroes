using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using Code.UI.Components;

public class NavigateMenuItem : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction<NavigateMenuItem> OnClick;
    public event UnityAction OnSelect;
    public event UnityAction OnDeselect;

    [SerializeField]
    private TweenSequence m_SelectSequence;
    [SerializeField]
    private TweenSequence m_DeselectSequence;
    [SerializeField]
    private RectTransform m_Lock;
    [SerializeField]
    private RectTransform m_Icon;

    private bool m_IsSelected;
    public bool isSelected => m_IsSelected;
    [Button]
    public void SetSelect(bool value, bool immediately)
    {
        if(value) m_SelectSequence.Play(immediately);
        else m_DeselectSequence.Play(immediately);
        m_IsSelected = value;
    }

    public bool m_IsLock;
    public bool isLock => m_IsLock;

    [Button]
    public void SetLock(bool value, bool immediately)
    {
        m_Lock.gameObject.SetActive(value);
        m_Icon.gameObject.SetActive(!value);
        m_IsLock = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (m_IsLock)
        {
            m_Lock.DOKill();
            m_Lock.localPosition = Vector3.zero;
            m_Lock.DOPunchPosition(Vector3.up * 20, 0.5f, 7);
        }
        else
        {
            OnClick?.Invoke(this);
        }
    }
}
