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

	public string productId => m_ProductID;

	public void OnInitialize(Product product, IAPShopService shopService)
	{
		_isBoughtNoAds = new PlayerPrefsProperty<bool>(PlayerPrefsProperty.ToKey(nameof(IAPNoAdsProcessor), nameof(_isBoughtNoAds))).Build();

		if (_isBoughtNoAds.value) return;
		
		var categoryView = ServiceLocator.Get<UIRoot>().GetWidget<IAPShopWidget>()
	.GetCategory<IAPCategoryView>(_category);

		m_ShopService = shopService;

		_noAdsViewCopy = Instantiate(_noAdsViewPrefab);
		_noAdsViewCopy.SetPrice(product.metadata.localizedPriceString);
		_noAdsViewCopy.Clicked += InitiatePurchase;

		categoryView.AddProductView(_noAdsViewCopy);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		ServiceLocator.ForEach<AdsInterstitial>(ads => ads.IsAllowedToShowAds = false);
		_isBoughtNoAds.value = true;
		Destroy(_noAdsViewCopy.gameObject);
		return PurchaseProcessingResult.Complete;
	}

	private void OnChangeId()
	{
		gameObject.name = productId;
	}

	private void InitiatePurchase()
	{
		_noAdsViewCopy.Clicked -= InitiatePurchase;
		m_ShopService.InitiatePurchase(m_ProductID);
	}
}
