using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventsUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	public event Action<PointerEventData> PointerDown;
	public event Action<PointerEventData> PointerUp;
	public event Action<PointerEventData> PointerClick;

	public void OnPointerClick(PointerEventData eventData)
	{
		PointerClick?.Invoke(eventData);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PointerDown?.Invoke(eventData);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		PointerUp?.Invoke(eventData);
	}
}
