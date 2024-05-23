using Code.Services;
using Extensions;
using Game.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class InAppCompletedPopUpWidget : MonoBehaviour, IUIElement
{
	[SerializeField] private PointerEventsUI m_pointerEventsUI;
	[SerializeField] private Transform m_spawnParent;
	[SerializeField] private GameObject m_separatorPrefab;
	[SerializeField, OdinSerialize] private List<PopUpPrefab> m_prefabs;

	private List<PopUpItem> m_activePopUpItems;
	private List<GameObject> m_seperators;

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
		var incomeAnimation = ServiceLocator.Get<IncomeAnimationService>();

		gameObject.Disable();

		m_activePopUpItems ??= new List<PopUpItem>();
		m_seperators ??= new List<GameObject>();

		for (int i = 0; i < m_activePopUpItems.Count; i++)
		{
			if (!immediately)
			{
				if (m_activePopUpItems is IIAPAnimatedPopUp iAPAnimatedPopUp)
				{
					iAPAnimatedPopUp.Animate(incomeAnimation);
				}
			}

			Destroy(m_activePopUpItems[i].gameObject);
		}

		for (int i = 0; i < m_seperators.Count; i++)
		{
			Destroy(m_seperators[i]);
		}

		m_activePopUpItems.Clear();
		m_seperators.Clear();
	}

	public void Show(bool immediately)
	{
		gameObject.Enable();
	}

	public void Initialise(params PopUpItemData[] popUpItemDatas)
	{
		Initialise((IEnumerable<PopUpItemData>)popUpItemDatas);
	}

	public void Initialise(IEnumerable<PopUpItemData> popUpItemDatas)
	{
		m_activePopUpItems ??= new List<PopUpItem>();
		m_seperators ??= new List<GameObject>();

		var length = popUpItemDatas.Count();

		for (int i = 0; i < length; i++)
		{
			Add(popUpItemDatas.ElementAt(i));
			if (i < length - 1)
			{
				AddSeparator();
			}
		}
	}

	public void Add(PopUpItemData popUpItemData)
	{
		var prefab = m_prefabs.Find(p => p.PopUpItemType == popUpItemData.PopUpItemType);
		var popUpItemCopy = Instantiate(prefab.PopUpItem, m_spawnParent);
		popUpItemCopy.Initialise(popUpItemData);

		m_activePopUpItems.Add(popUpItemCopy);
	}

	[Button]
	public void Test1()
	{
		Initialise(new PopUpItemData(null, "999999999", null, PopUpItemType.Coins_soft));
	}

	[Button]
	public void Test2()
	{
		Initialise(
			new ShipPopUpData(null, "ship", null, PopUpItemType.Ship, "class"),
			new PopUpItemData(null, "9000000", null, PopUpItemType.Coins_soft));
	}

	private void OnPointerClick(UnityEngine.EventSystems.PointerEventData obj)
	{
		Hide(true);
	}

	private void AddSeparator()
	{
		var separatorCopy = Instantiate(m_separatorPrefab, m_spawnParent);
		m_seperators.Add(separatorCopy);
	}
}
