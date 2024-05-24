using Code.Game.Wallet;
using UnityEngine;

public class PopUpItemData
{
	public Sprite Sprite;
	public object Title;
	public Currency Currency;
	public PopUpItemType PopUpItemType;

	public PopUpItemData(Sprite sprite, object title, Currency currency, PopUpItemType popUpItemType)
	{
		Sprite = sprite;
		Title = title;
		Currency = currency;
		PopUpItemType = popUpItemType;
	}
}
