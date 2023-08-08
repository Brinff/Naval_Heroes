using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;
using UnityEngine.UIElements;

public class PointerInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerPerformDelegate(PointerEventData eventData);

    public static event PointerPerformDelegate OnClick;
    public static event PointerPerformDelegate OnDown;
    public static event PointerPerformDelegate OnUp;
    public static event PointerPerformDelegate OnEnter;
    public static event PointerPerformDelegate OnExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke(eventData);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp?.Invoke(eventData); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit?.Invoke(eventData);
    }
}
