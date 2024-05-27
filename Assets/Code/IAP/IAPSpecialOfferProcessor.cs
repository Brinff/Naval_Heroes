using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Code.Game;
using Code.Game.Analytics;
using Code.Game.Slots.Stash;
using Code.Game.Wallet;
using Code.IAP.Attributes;
using Code.IO;
using Code.Services;
using Code.UI.Widgets;
using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.IAP
{
    public class IAPSpecialOfferProcessor : MonoBehaviour, IIAPProcessor
    {
        [ProductId, SerializeField, OnValueChanged(nameof(OnChangeId))]
        private string m_ProductID;

        public string productId => m_ProductID;

        [SerializeField] private EntityData m_Ship;
        [SerializeField] private int m_Amount;
        [SerializeField] private Currency m_Currency;

        [SerializeField] private IAPCategory m_Category;
        [SerializeField] private GameObject m_CardPrefab;
        [SerializeField] private GameObject m_BannerPrefab;

        private PlayerPrefsProperty<bool> m_IsPurchased;
        private PlayerPrefsProperty<long> m_StartDateTime;

        private IAPShopService m_ShopService;
        private WalletService m_WalletService;
        private StashService m_StashService;
        private IAPSpecialOfferView m_CardView;
        private IAPSpecialOfferView m_BannerView;

        [SerializeField] private int m_Hours;

        private DateTime m_DataTime;

        [SerializeField] private float m_OldPriceMultiplier = 1;


        private void OnChangeId()
        {
            gameObject.name = m_ProductID;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            m_WalletService.IncomeValue(m_Amount, AnalyticService.IAP, m_ProductID);
            m_StashService.AddItem(m_Ship);
            m_StashService.NeedInspect();
            
            m_IsPurchased.value = true;
            
            Destroy(m_CardView.gameObject);
            Destroy(m_BannerView.gameObject);
            
            m_IsInitialized = false;

            var popUp = ServiceLocator.Get<UIRoot>().GetWidget<InAppCompletedPopUpWidget>();
            popUp.Initialise
                (
                    new ShipPopUpData("S.DAKOTA", PopUpItemType.Ship, "VIII"),
                    new PopUpItemData(m_Amount, PopUpItemType.Coins_soft)
                );
            popUp.Show(false);

            ServiceLocator.Get<NavigateMediator>().Select("Fleet", false);
            
            return PurchaseProcessingResult.Complete;
        }

        private void InitiatePurchase()
        {
            m_ShopService.InitiatePurchase(m_ProductID);
        }

        private void ShowShop()
        {
            ServiceLocator.Get<NavigateMediator>().Select("Shop", false);
        }

        private bool m_IsInitialized;

        public void OnInitialize(Product product, IAPShopService shopService)
        {
            m_IsPurchased = new PlayerPrefsProperty<bool>(PlayerPrefsProperty.ToKey(nameof(IAPSpecialOfferProcessor),
                    m_ProductID, nameof(m_IsPurchased)))
                .Build();

            if (m_IsPurchased.value) return;

            m_IsInitialized = true;
            m_ShopService = shopService;
            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
            m_StashService = ServiceLocator.Get<StashService>();


            var categoryView = ServiceLocator.Get<UIRoot>().GetWidget<IAPShopWidget>()
                .GetCategory<IAPCategoryView>(m_Category);
            
            m_CardView = Instantiate(m_CardPrefab).GetComponent<IAPSpecialOfferView>();
            m_CardView.titleLabel.text = product.metadata.localizedTitle;
            m_CardView.currentCostLabel.text = product.metadata.localizedPriceString;
            m_CardView.oldCostLabel.text = GetOldPrice(product);
            m_CardView.rewardLabel.SetValue(m_Amount, true);
            m_CardView.button.OnClick += InitiatePurchase;
            categoryView.AddProductView(m_CardView);

            var bannerView = ServiceLocator.Get<UIRoot>().GetWidget<TopBannerLayoutWidget>();
            m_BannerView = Instantiate(m_BannerPrefab).GetComponent<IAPSpecialOfferView>();
            m_BannerView.titleLabel.text = product.metadata.localizedTitle;
            m_BannerView.currentCostLabel.text = product.metadata.localizedPriceString;
            m_BannerView.oldCostLabel.text = GetOldPrice(product);
            m_BannerView.rewardLabel.SetValue(m_Amount, true);
            m_BannerView.button.OnClick += ShowShop;
            bannerView.AddBannerView(m_BannerView);

            m_StartDateTime = new PlayerPrefsProperty<long>(PlayerPrefsProperty.ToKey(nameof(IAPSpecialOfferProcessor),
                    m_ProductID, nameof(m_StartDateTime)))
                .OnDefault(() => DateTime.Now.Ticks)
                .Build();
            m_StartDateTime.Save();
        }

        private TimeSpan GetLeftTime()
        {
            return new TimeSpan(m_Hours, 0, 0) - DateTime.Now.Subtract(new DateTime(m_StartDateTime.value));
        }

        private string GetOldPrice(Product product)
        {
            string localizedPriceString = product.metadata.localizedPriceString;
            var currentPrice = (float)product.metadata.localizedPrice;
            var oldPrice = Mathf.Round(currentPrice * m_OldPriceMultiplier) - 0.01f;
            var regex = new Regex(@"[\d,.]+");
            return regex.Replace(localizedPriceString, oldPrice.ToString(CultureInfo.InvariantCulture));
        }


        private void Update()
        {
            if (!m_IsInitialized) return;
            
            var timeSpan = GetLeftTime();
            if (timeSpan.TotalSeconds < 0)
            {
                m_StartDateTime.value = DateTime.Now.Ticks;
            }
            string time = timeSpan.ToString("hh\\:mm\\:ss");
            
            m_CardView.timeLabel.text = time;
            m_BannerView.timeLabel.text = time;
        }
    }
}