using Extensions;
using Game.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPNoAdsView : MonoBehaviour, IUIElement
{
	[SerializeField] private TMP_Text _price;
	[SerializeField] private TMP_Text _title;
	[SerializeField] private Button _button;

	public event Action Clicked;

	private void Awake()
	{
		_button.onClick.AddListener(OnButtonClick);
	}

	public void Hide(bool immediately)
	{
		gameObject.Disable();
	}

	public void Show(bool immediately)
	{
		gameObject.Enable();
	}

	public IAPNoAdsView SetPrice(string price)
	{
		_price.SetText(price);
		return this;
	}

	public IAPNoAdsView SetTitle(string title)
	{
		_title.SetText(title);
		return this;
	}

	private void OnButtonClick()
	{
		Clicked?.Invoke();
	}
}
