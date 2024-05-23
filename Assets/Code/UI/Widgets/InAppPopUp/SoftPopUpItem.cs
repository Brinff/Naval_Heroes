using Code.Services;
using Game.UI;
using UnityEngine;

public class SoftPopUpItem : PopUpItem, IIAPAnimatedPopUp
{
	public void Animate(IncomeAnimationService incomeAnimationService)
	{
		var softMoneyRect = ServiceLocator.Get<UIRoot>().GetWidget<SoftMoneyCounterWidget>().GetComponent<RectTransform>();
		incomeAnimationService.AnimateScreenSpace(RectTransform, softMoneyRect);
	}

	public override void SetTitle(string title)
	{
		base.SetTitle(title);
	}
}