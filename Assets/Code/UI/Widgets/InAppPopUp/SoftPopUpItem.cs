using Code.Services;
using Game.UI;
using System.Globalization;
using UnityEngine;

public class SoftPopUpItem : PopUpItem, IIAPAnimatedPopUp
{
	[SerializeField] private Sprite m_icon;

	public void Animate(IncomeAnimationService incomeAnimationService)
	{
		var softMoneyRect = ServiceLocator.Get<UIRoot>().GetWidget<SoftMoneyCounterWidget>().GetComponent<RectTransform>();
		incomeAnimationService.AnimateScreenSpace(RectTransform, softMoneyRect);
	}

	public override void SetTitle(object title)
	{
		var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
		nfi.NumberGroupSeparator = " ";
		var s = title.ToString();

		var value = int.Parse(title.ToString());
		m_title.SetText(value.ToString("#,#", nfi));
	}
}