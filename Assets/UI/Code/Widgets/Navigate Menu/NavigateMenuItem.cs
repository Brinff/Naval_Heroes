using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

public class NavigateMenuItem : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction OnClick;
    public event UnityAction OnSelect;
    public event UnityAction OnDeselect;

    

    public enum State
    {
        Lock, Select, Normal
    }

    private State m_State;

    public State state => m_State;

    [Button]
    public void SetState(State state, bool immediately)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
