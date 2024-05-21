using System;
using Code.Diagnostic;
using Code.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Game.Wallet
{
    public class WalletDiagnostic : MonoBehaviour, IDiagnostic, IInitializable
    {
        [SerializeField] private Currency m_Currency;

        public int order => 0;
        public string path => $"Wallets/{m_Currency.name}";

        private void OnEnable()
        {
            DiagnosticService.Register(this);
        }

        private void OnDisable()
        {
            DiagnosticService.Unregister(this);
        }

        private bool m_IsCreated;
        public VisualElement CreateVisualTree()
        {
            m_IsCreated = true;
            var root = new Group(m_Currency.name, FlexDirection.Column);

            VisualElement moneyGroup = new VisualElement();
            moneyGroup.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            root.Add(moneyGroup);
            
            CreateAddMoneyButton(moneyGroup, -1000);
            CreateAddMoneyButton(moneyGroup, -100);
            CreateAddMoneyButton(moneyGroup, -10);

            m_WalletField = new IntegerField();
            m_WalletField.value = GetWalletMoneyValue();
            m_WalletField.RegisterValueChangedCallback(InputWalletMoneyValue);
            m_WalletField.style.flexGrow = 1;

            moneyGroup.Add(m_WalletField);

            CreateAddMoneyButton(moneyGroup, 10);
            CreateAddMoneyButton(moneyGroup, 100);
            CreateAddMoneyButton(moneyGroup, 1000);
            
            m_TotalIncome = new Label("Total Income: #$");
            root.Add(m_TotalIncome);
            m_TotalSpend = new Label("Total Spend: #$");
            root.Add(m_TotalSpend);

            return root;
        }

        private WalletService m_WalletService;
        private IntegerField m_WalletField;
        private Label m_TotalIncome;
        private Label m_TotalSpend;
        

        private void InputWalletMoneyValue(ChangeEvent<int> value)
        {
            m_WalletService.SetValue(value.newValue);
        }

        private int GetWalletMoneyValue()
        {
            if (m_WalletService == null) return 0;
            return Mathf.RoundToInt(m_WalletService.amount);
        }

        private void CreateAddMoneyButton(VisualElement parent, int amount)
        {
            var button = new Button(() => AddMoneyToWallet(amount));
            button.text = amount > 0 ? $"+{amount}" : amount.ToString();
            parent.Add(button);
        }

        private void AddMoneyToWallet(int amount)
        {
            if (amount > 0) m_WalletService.IncomeValue(amount, "Game","Debug");
            if (amount < 0) m_WalletService.SpendValue(-amount, "Game","Debug");
        }

        private void Update()
        {
            if(!m_IsCreated)return;
            if (m_WalletService == null) return;
            if (m_WalletField != null && m_WalletField.panel != null &&
                m_WalletField.panel.focusController.focusedElement != m_WalletField)
                m_WalletField.SetValueWithoutNotify(GetWalletMoneyValue());

            m_TotalIncome.text = $"Income: {m_WalletService.totalIncomeAmount}";
            m_TotalSpend.text = $"Spend: {m_WalletService.totalSpendAmount}";
        }
        
        public void Initialize()
        {
            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
        }
    }
}