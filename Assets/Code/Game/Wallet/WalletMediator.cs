/*using System;
using System.Collections;
using System.Collections.Generic;
using Code.Game.Flow;*/
using Code.Services;
using Game.UI;
/*using Code.UI;
using UI.Code.Widgets;*/
using UnityEngine;

namespace Code.Game.Wallet
{

    public class WalletMediator : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private Currency m_Currency;
        private WalletService m_WalletService;
        private SoftMoneyCounterWidget m_WalletCounter;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);    
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }
        

        public void Initialize()
        {
            m_WalletService = ServiceLocator.Get<WalletService>(x => x.currency == m_Currency);
           
            m_WalletCounter = ServiceLocator.Get<UIService>().GetElement<SoftMoneyCounterWidget>();
            m_WalletService.OnUpdate += OnUpdateWallet;
            m_WalletCounter.SetMoney(m_WalletService.amount);
        }

        private void OnUpdateWallet()
        {
            m_WalletCounter.SetMoney(m_WalletService.amount);
        }
    }
}
