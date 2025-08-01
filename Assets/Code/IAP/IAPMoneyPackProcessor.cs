﻿using Code.Game;
using Code.Game.Analytics;
using Code.Game.Wallet;
using Code.IAP.Attributes;
using Code.Services;
using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Code.IAP
{
    public class IAPMoneyPackProcessor : MonoBehaviour, IIAPProcessor
    {
        [ProductId, SerializeField, OnValueChanged(nameof(OnChangeId))] private string m_ProductID;
        [SerializeField] private Sprite m_Icon;
        [SerializeField] private int m_Amount;
        [SerializeField] private int m_ValueBenefit;
        
        [SerializeField] private Currency m_Currency;
        [SerializeField] private IAPCategory m_Category;
        [SerializeField] private GameObject m_CardPrefab;
        
        private IAPShopService m_ShopService;
        private WalletService m_WalletService;

        private void OnChangeId()
        {
            gameObject.name = productId;
        }
        
        private void InitiatePurchase()
        {
            m_ShopService.InitiatePurchase(m_ProductID);
        }
        
        public string productId => m_ProductID;
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            m_WalletService.IncomeValue(m_Amount, AnalyticService.IAP, m_ProductID);

            var inAppPopUpWidget = ServiceLocator.Get<UIRoot>().GetWidget<InAppCompletedPopUpWidget>();
            inAppPopUpWidget.Initialise(new PopUpItemData(m_Amount, PopUpItemType.Coins_soft));
            ServiceLocator.Get<NavigateMediator>().Select("Fleet", false);
            inAppPopUpWidget.Show(false);
    
            return PurchaseProcessingResult.Complete;
        }

        public void OnInitialize(Product product, IAPShopService shopService)
        {
            m_ShopService = shopService;
            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
            var categoryView = ServiceLocator.Get<UIRoot>().GetWidget<IAPShopWidget>().GetCategory<IAPCategoryView>(m_Category);
            var productView = Instantiate(m_CardPrefab).GetComponent<IAPMoneyPackView>();
            productView.titleLabel.text = product.metadata.localizedTitle;
            productView.costLabel.text = product.metadata.localizedPriceString;
            productView.valueLabel.gameObject.SetActive(m_ValueBenefit > 0);
            productView.valueLabel.SetValue(m_ValueBenefit, true);
            productView.rewardLabel.SetValue(m_Amount, true);
            productView.iconImage.sprite = m_Icon;
            productView.button.OnClick += InitiatePurchase;
            categoryView.AddProductView(productView);
        }
    }
}