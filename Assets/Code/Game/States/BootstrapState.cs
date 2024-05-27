
using Code.Game.Ads;
using Code.Ads;
using Code.Diagnostic;
using Code.Game.Analytics;
using Code.Game.Slots;
using Code.Game.Slots.Battle;
using Code.Game.Slots.Buy;
using Code.Game.Slots.Merge;
using Code.Game.Slots.Stash;
using Code.Game.Wallet;
using Code.Services;
using Code.States;
using UnityEngine;
using Assets.Code.Game;
using Code.IAP;
using Game.UI;

namespace Code.Game.States
{
    public class BootstrapState : MonoBehaviour, IPlayState
    {
        [SerializeField] private bool m_LaunchGameAtStart;

        private IStateMachine m_StateMachine;
        public void OnPlay(IStateMachine stateMachine)
        {
            //TODO: All initializes, sdk, loads, ect
            Application.targetFrameRate = 60;
            
            
            m_StateMachine = stateMachine;
            
            ServiceLocator.ForEach<UIRoot>(x=>x.Initialize());
            
            ServiceLocator.ForEach<AnalyticService>(x=>x.Initialize());
            ServiceLocator.ForEach<AdsService>(x=>x.Initialize());
            
            ServiceLocator.ForEach<WalletService>(x => x.Initialize());
            ServiceLocator.ForEach<WalletMediator>(x => x.Initialize());
            
            ServiceLocator.ForEach<ItemFactory>(x=>x.Initialize());
            
            ServiceLocator.ForEach<BuyAdsShipService>(x=>x.Initialize());
            ServiceLocator.ForEach<BuyCurrencyShipService>(x=>x.Initialize());
            ServiceLocator.ForEach<BuyShipMediator>(x=>x.Initialize());
            
            ServiceLocator.ForEach<MergeService>(x=>x.Initialize());
            ServiceLocator.ForEach<MergeMediator>(x=>x.Initialize());
            
           
            
            ServiceLocator.ForEach<BattleFieldService>(x=>x.Initialize());
            ServiceLocator.ForEach<BattleFieldMediator>(x=>x.Initialize());
            //ServiceLocator.ForEach<BattleFieldView>(x=>x);

            ServiceLocator.ForEach<NavigateMediator>(x => x.Initialize());

            ServiceLocator.ForEach<IAPShopService>(x => x.Initialize());
            ServiceLocator.ForEach<IAPShopMediator>(x => x.Initialize());



            ServiceLocator.ForEach<EntityManager>(x=>x.Initialize());

            ServiceLocator.ForEach<StashService>(x => x.Initialize());
            ServiceLocator.ForEach<StashMediator>(x => x.Initialize());

            ServiceLocator.ForEach<DiagnosticService>(x => x.Initialize());
            
            if (m_LaunchGameAtStart) m_StateMachine.Play<LaunchGameState>();
        }
    }
}