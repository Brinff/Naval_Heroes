using System;
using System.Collections.Generic;
using Assets.Code.Game.Wallet;
using Code.CSV;
using Code.Game.Analytics;
using Code.IO;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Game.Wallet
{
    public class WalletService : MonoBehaviour, IService
    {
        [SerializeField] private Currency m_Currency;
        [SerializeField] private string m_CustomKey;

        [SerializeField] private int m_StartValue;
        public Currency currency => m_Currency;

        [SerializeField] private CSVTable m_Log;
        [ShowInInspector] public int amount => m_Amount != null ? m_Amount.value : 0;

        private AnalyticService m_AnalyticService;
        
        public delegate void AmountDelegate();

        private bool m_IsDirty;

        public event AmountDelegate OnChangeAmount;
        public event AmountDelegate OnUpdate;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }


        public bool isInitialized { get; private set; }

        private PlayerPrefsProperty<int> m_Amount;
        private PlayerPrefsProperty<int> m_TotalSpendAmount;
        private PlayerPrefsProperty<int> m_TotalIncomeAmount;

        public int totalSpendAmount => m_TotalSpendAmount.value;
        public int totalIncomeAmount => m_TotalIncomeAmount.value;

        private string GetSaveKey(string key)
        {
            return PlayerPrefsProperty.ToKey($"{m_Currency.name}_{key}");
        }

        public void Initialize()
        {
            if (isInitialized) return;
            isInitialized = true;

            m_AnalyticService = ServiceLocator.Get<AnalyticService>();

            m_TotalSpendAmount = new PlayerPrefsProperty<int>(GetSaveKey(nameof(m_TotalSpendAmount))).Build();
            m_TotalIncomeAmount = new PlayerPrefsProperty<int>(GetSaveKey(nameof(m_TotalIncomeAmount))).Build();
            
            m_Amount = new PlayerPrefsProperty<int>(string.IsNullOrEmpty(m_CustomKey) ? GetSaveKey(nameof(m_Amount)) : m_CustomKey).OnDefault(() =>
            {
                var walletMigration = GetComponent<IWalletMigration>();
                if (walletMigration?.Migrate(out int migrateValue) ?? false)
                {
                    Log(0, m_StartValue, m_StartValue, Operation.Income, AnalyticService.GAME, "Migrate");
                    m_AnalyticService.OnCurrencyGiven(m_Currency.name, m_StartValue, AnalyticService.GAME, "Migrate");
                    return migrateValue;
                }
                else
                {
                    Log(0, m_StartValue, m_StartValue, Operation.Income, AnalyticService.GAME, "FirstLaunch");
                    m_AnalyticService.OnCurrencyGiven(m_Currency.name, m_StartValue, AnalyticService.GAME, "FirstLaunch");
                    return m_StartValue;
                }
            }).Build();


        }

        public bool IsEnough(int amount)
        {
            return m_Amount.value >= amount;
        }

        public void SetValue(int amount)
        {
            m_Amount.value = amount;
            OnChangeAmount?.Invoke();
            m_IsDirty = true;
        }

        [Button]
        public void IncomeValue(int amount, string source, string id)
        {
            m_TotalIncomeAmount.value += amount;

            Log(m_Amount.value, amount, m_Amount.value + amount, Operation.Income, source, id);
            
            m_AnalyticService.OnCurrencyTaken(m_Currency.name, amount, source, id);
            
            m_Amount.value += amount;

            OnChangeAmount?.Invoke();
            m_IsDirty = true;
        }

        public enum Operation
        {
            Income,
            Spend
        }

        private void Log(int beforeValue, int value, int afterValue, Operation operation, string source, string id)
        {
#if UNITY_EDITOR
            if (!m_Log.isExists)
            {
                m_Log.AddLine("Data", "Before Value", "Income", "Spend", "After Value", "TotalIncome", "TotalSpend",
                    "Source", "Id");
            }

            if (operation == Operation.Income)
                m_Log.AddLine(System.DateTime.Now, beforeValue, value, 0, afterValue, totalIncomeAmount,
                    totalSpendAmount, source, id);
            if (operation == Operation.Spend)
                m_Log.AddLine(System.DateTime.Now, beforeValue, 0, value, afterValue, totalIncomeAmount,
                    totalSpendAmount, source, id);
#endif
        }

        [Button]
        public void SpendValue(int amount, string source, string id)
        {
            m_TotalSpendAmount.value += amount;

            Log(m_Amount.value, amount, m_Amount.value - amount, Operation.Spend, source, id);
            
            m_AnalyticService.OnCurrencyTaken(m_Currency.name, amount, source, id);
            
            m_Amount.value -= amount;

            if (m_Amount.value < 0) m_Amount.value = 0;
            OnChangeAmount?.Invoke();
            m_IsDirty = true;
        }

        private void Update()
        {
            if (m_IsDirty)
            {
                m_IsDirty = true;
                OnUpdate?.Invoke();
            }
        }
    }
}