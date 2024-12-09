using Extensions;
using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsMainWidget : MonoBehaviour, IUIElement
{
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

	private void OnButtonClick()
	{
		Clicked?.Invoke();
	}
}