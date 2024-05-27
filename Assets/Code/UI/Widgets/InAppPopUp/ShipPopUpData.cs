using Code.Game.Wallet;
using UnityEngine;

public class ShipPopUpData : PopUpItemData
{
	public string ShipClassText;

	public ShipPopUpData(Sprite sprite, string title, Currency currency, PopUpItemType popUpItemType, string shipClassText) : base(sprite, title, currency, popUpItemType)
	{
		ShipClassText = shipClassText;
	}
}
