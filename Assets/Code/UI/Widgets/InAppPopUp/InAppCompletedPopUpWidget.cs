using Extensions;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppCompletedPopUpWidget : MonoBehaviour, IUIElement
{
	[SerializeField] private PopUpItem m_itemPrefab;
	[SerializeField] private PointerEventsUI m_pointerEventsUI;
	[SerializeField] private Transform m_spawnParent;

	private List<PopUpItem> m_activePopUpItems;

	private void OnEnable()
	{
		m_pointerEventsUI.PointerClick += OnPointerClick;
	}

	private void OnDisable()
	{
		m_pointerEventsUI.PointerClick -= OnPointerClick;
	}

	public void Hide(bool immediately)
	{
		gameObject.Disable();
		for (int i = 0; i < m_activePopUpItems.Count; i++)
		{
			Destroy(m_activePopUpItems[i].gameObject);
		}
	}

	public void Show(bool immediately)
	{
		gameObject.Enable();
	}

	public void Initialse(List<PopUpItemData> popUpItemDatas)
	{
		m_activePopUpItems ??= new List<PopUpItem>();

		foreach (var popUpData in popUpItemDatas)
		{
			Add(popUpData);
		}
	}

	public void Add(PopUpItemData popUpItemData)
	{
		var popUpItemCopy = Instantiate(m_itemPrefab, m_spawnParent);
		popUpItemCopy.Initialise(popUpItemData);

		m_activePopUpItems.Add(popUpItemCopy);
	}

	private void OnPointerClick(UnityEngine.EventSystems.PointerEventData obj)
	{
		Hide(true);
	}
}
