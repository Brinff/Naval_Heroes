using Code.Ads;
using Code.IAP;
using Code.IAP.Attributes;
using Code.IO;
using Code.Services;
using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPNoAdsProcessor : MonoBehaviour, IIAPProcessor
{
	[ProductId, SerializeField, OnValueChanged(nameof(OnChangeId))] private string m_ProductID;
	[SerializeField] private IAPNoAdsView _noAdsViewPrefab;
	[SerializeField] private IAPCategory _category;

	private PlayerPrefsProperty<bool> _isBoughtNoAds;
	private IAPShopService m_ShopService;
	private IAPNoAdsView _noAdsViewCopy;
	private NoAdsMainWidget _noAdsMainWidget;
	private UIRoot _uiRoot;

	public string productId => m_ProductID;

	public void OnInitialize(Product product, IAPShopService shopService)
	{
		_isBoughtNoAds = new PlayerPrefsProperty<bool>(PlayerPrefsProperty.ToKey(nameof(IAPNoAdsProcessor), nameof(_isBoughtNoAds))).Build();
		_uiRoot = ServiceLocator.Get<UIRoot>();
		_noAdsMainWidget = _uiRoot.GetWidget<NoAdsMainWidget>();

		if (_isBoughtNoAds.value)
		{
			ServiceLocator.Get<UICompositionController>().GetComposition<UIHomeComposition>().Remove(_noAdsMainWidget);
			ServiceLocator.ForEach<AdsInterstitial>(ads => ads.IsAllowedToShowAds = false);
			Destroy(_noAdsMainWidget.gameObject);
			return;
		}
		

		var categoryView = ServiceLocator.Get<UIRoot>().GetWidget<IAPShopWidget>()
	.GetCategory<IAPCategoryView>(_category);

		m_ShopService = shopService;

		_noAdsViewCopy = Instantiate(_noAdsViewPrefab);
		_noAdsViewCopy.SetPrice(product.metadata.localizedPriceString).SetTitle(product.metadata.localizedTitle);
		_noAdsViewCopy.Clicked += InitiatePurchase;
		_noAdsMainWidget.Clicked += InitiatePurchase;
		categoryView.AddProductView(_noAdsViewCopy);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		_noAdsViewCopy.Clicked -= InitiatePurchase;
		_noAdsMainWidget.Clicked -= InitiatePurchase;

		ServiceLocator.ForEach<AdsInterstitial>(ads => ads.IsAllowedToShowAds = false);
		ServiceLocator.Get<UICompositionController>().GetComposition<UIHomeComposition>().Remove(_noAdsMainWidget);
		_isBoughtNoAds.value = true;

		Destroy(_noAdsViewCopy.gameObject);
		Destroy(_noAdsMainWidget.gameObject);

		return PurchaseProcessingResult.Complete;
	}

	private void OnChangeId()
	{
		gameObject.name = productId;
	}

	private void InitiatePurchase()
	{
		m_ShopService.InitiatePurchase(m_ProductID);
	}
}
