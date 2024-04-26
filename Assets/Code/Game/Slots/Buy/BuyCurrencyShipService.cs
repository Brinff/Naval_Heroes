using System;
using Code.Game.Analytics;
using Code.Game.Wallet;
using Code.IO;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Game.Slots.Buy
{
    public class BuyCurrencyShipService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField] private ProgressionData m_ProgressionCost;
        [SerializeField] private Category m_Category;
        public Category category => m_Category;
        [SerializeField] private Currency m_Currency;

        private WalletService m_WalletService;
        private AnalyticService m_AnalyticService;

        private PlayerPrefsProperty<int> m_TotalCountTransaction;

        public int cost => (int)m_ProgressionCost.GetResult(m_TotalCountTransaction.value);


        private string id => $"Buy_{m_Category.name}";

        private bool m_IsDirty;

        public delegate void UpdateDelegate();

        public event UpdateDelegate OnUpdate;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public bool IsEnoughCurrency()
        {
            return m_WalletService.IsEnough(cost);
        }

        private int GetDefaultTransaction()
        {
            string name = $"m_PlayerAmountBuyShip{m_Category.name}";
            PlayerPrefsData<int> oldData = new PlayerPrefsData<int>(name);
            return oldData.HasValue() ? oldData.Value : 0;
        }

        public void Initialize()
        {
            m_TotalCountTransaction = new PlayerPrefsProperty<int>(
                    PlayerPrefsProperty.ToKey(nameof(BuyCurrencyShipService), m_Category.name, nameof(m_TotalCountTransaction)))
                .OnDefault(GetDefaultTransaction)
                .Build();

            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
            m_AnalyticService = ServiceLocator.Get<AnalyticService>();

            m_WalletService.OnUpdate += OnUpdateWallet;

            m_AnalyticService.DeclareCurrency(m_Category.name);
        }

        private void OnUpdateWallet()
        {
            m_IsDirty = true;
        }

        [Button]
        public void Buy()
        {
            if (IsEnoughCurrency())
            {
                m_WalletService.SpendValue(cost, AnalyticService.GAME, id);
                m_AnalyticService.OnCurrencyGiven(m_Category.name, 1, AnalyticService.GAME, id);
                m_TotalCountTransaction.value++;
                m_IsDirty = true;
            }
        }

        private void Update()
        {
            if (m_IsDirty)
            {
                OnUpdate?.Invoke();
                m_IsDirty = false;
            }
        }
    }
}