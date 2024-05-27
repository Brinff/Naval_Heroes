using Code.Services;
using Code.UI.Widgets.Stash;
using DG.Tweening;
using Extensions;
using Game.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ShipPopUpItem : PopUpItem, IIAPAnimatedPopUp
{
	[SerializeField] private TextMeshProUGUI m_shipLevel;

	public async Task Animate(IncomeAnimationService incomeAnimationService)
	{
		var destinationRectTransform = ServiceLocator.Get<UIRoot>().GetWidget<StashButtonWidget>().GetComponent<RectTransform>();
		await incomeAnimationService.AnimateMovement(RectTransform, destinationRectTransform, true, true);
		await destinationRectTransform.DOPulse(loops: 0).AsyncWaitForCompletion();
	}

	public override void Initialise(PopUpItemData popUpItemData)
	{
		base.Initialise(popUpItemData);
		if (popUpItemData is ShipPopUpData shipPopUpData)
		{
			m_shipLevel.SetText(shipPopUpData.ShipClassText);
		}
	}
}
