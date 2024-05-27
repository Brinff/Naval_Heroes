public class ShipPopUpData : PopUpItemData
{
	public string ShipClassText;

	public ShipPopUpData(string title, PopUpItemType popUpItemType, string shipClassText) : base(title, popUpItemType)
	{
		ShipClassText = shipClassText;
	}
}
