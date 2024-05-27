using TMPro;
using UnityEngine;

public class ShipPopUpItem : PopUpItem
{
	[SerializeField] private TextMeshProUGUI m_shipLevel;

	public override void Initialise(PopUpItemData popUpItemData)
	{
		base.Initialise(popUpItemData);
		if (popUpItemData is ShipPopUpData shipPopUpData)
		{
			m_shipLevel.SetText(shipPopUpData.ShipClassText);
		}
	}
}
