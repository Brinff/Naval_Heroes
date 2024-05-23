using Code.Game.Wallet;
using UnityEngine;

public class PopUpItemData
{
	public Sprite Sprite;
	public string Title;
	public Currency Currency;
	public PopUpItemType PopUpItemType;

	public PopUpItemData(Sprite sprite, string title, Currency currency, PopUpItemType popUpItemType)
	{
		Sprite = sprite;
		Title = title;
		Currency = currency;
		PopUpItemType = popUpItemType;
	}
}
