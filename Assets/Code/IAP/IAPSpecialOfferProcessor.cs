using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Code.Game.Analytics;
using Code.Game.Slots.Stash;
using Code.Game.Wallet;
using Code.IAP.Attributes;
using Code.IO;
using Code.Services;
using Code.Utility;
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

        private PlayerPrefsProperty<bool> m_IsPurchased;
        private PlayerPrefsProperty<long> m_StartDateTime;

        private IAPShopService m_ShopService;
        private WalletService m_WalletService;
        private StashService m_StashService;
        private IAPSpecialOfferView m_ProductView;

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
            m_IsPurchased.value = true;
            Destroy(m_ProductView.gameObject);
            m_IsInitialized = false;
            ServiceLocator.Get<UICompositionController>().Show<UIHomeComposition>();
            return PurchaseProcessingResult.Complete;
        }

        private void InitiatePurchase()
        {
            m_ShopService.InitiatePurchase(m_ProductID);
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
            m_ProductView = Instantiate(m_CardPrefab).GetComponent<IAPSpecialOfferView>();
            m_ProductView.titleLabel.text = product.metadata.localizedTitle;
            m_ProductView.currentCostLabel.text = product.metadata.localizedPriceString;
            m_ProductView.oldCostLabel.text = GetOldPrice(product);
            m_ProductView.rewardLabel.SetValue(m_Amount, true);
            m_ProductView.button.OnClick += InitiatePurchase;
            categoryView.AddProductView(m_ProductView);


            m_StartDateTime = new PlayerPrefsProperty<long>(PlayerPrefsProperty.ToKey(nameof(IAPSpecialOfferProcessor),
                    m_ProductID, nameof(m_StartDateTime)))
                .OnDefault(() => DateTime.Now.Ticks)
                .Build();
            m_StartDateTime.Save();
            //m_DataTime = DateTime.Now.Ticks;
        }

        public TimeSpan GetLeftTime()
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

            m_ProductView.timeLabel.text = timeSpan.ToString("hh\\:mm\\:ss");
        }
    }
}