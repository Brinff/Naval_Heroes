using Code.Services;
using Game.UI;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

public class SoftPopUpItem : PopUpItem, IIAPAnimatedPopUp
{
	[SerializeField] private Sprite m_icon;

	public async Task Animate(IncomeAnimationService incomeAnimationService)
	{
		var softMoneyRect = ServiceLocator.Get<UIRoot>().GetWidget<SoftMoneyCounterWidget>().GetComponent<RectTransform>();
		await incomeAnimationService.AnimateCoinsScreenSpace(RectTransform, softMoneyRect);
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