using System;
using Code.Game.Wallet;
using Code.Services;
using UnityEngine;

namespace Code.Game.Slots
{
    public class BuyShipService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField] private ProgressionData m_ProgressionCost;
        [SerializeField] private ClassificationData m_Classification;
        public ClassificationData classification => m_Classification;
        [SerializeField] private Currency m_Currency;

        private WalletService m_WalletService;

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
            return m_WalletService.IsEnough((int)m_ProgressionCost.GetResult(1));
        }
        
        public void Initialize()
        {
            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
        }
    }
}