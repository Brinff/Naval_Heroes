using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CameraInputWidget : MonoBehaviour, IUIElement, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void CameraInputDelegate(Vector2 delta);

    public event CameraInputDelegate OnBeginInput;
    public event CameraInputDelegate OnUpdateInput;
    public event CameraInputDelegate OnEndInput;


    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }
    private PointerEventData pointerEventData;
    public void OnBeginDrag(PointerEventData eventData)
    {
        pointerEventData = eventData;
        OnBeginInput?.Invoke(eventData.delta);
    }

    private void Update()
    {
        if (pointerEventData != null)
            OnUpdateInput?.Invoke(pointerEventData.delta);
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointerEventData = eventData;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndInput?.Invoke(eventData.delta);
        pointerEventData = null;
    }
}
