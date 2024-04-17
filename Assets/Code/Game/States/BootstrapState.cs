
using Code.Diagnostic;
using Code.Game.Wallet;
using Code.Services;
using Code.States;
using UnityEngine;
using Voodoo.Tiny.Sauce.Internal.Ads;

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
            
            TSAdsManager.SetFSDisplayConditions(30, 30, 3);
            TSAdsManager.ToggleAds(true);
            
            m_StateMachine = stateMachine;
            
            ServiceLocator.ForEach<WalletService>(x => x.Initialize());
            ServiceLocator.ForEach<WalletMediator>(x => x.Initialize());
            
            ServiceLocator.ForEach<EntityManager>(x=>x.Initialize());
            
            ServiceLocator.ForEach<DiagnosticService>(x => x.Initialize());
            
            if (m_LaunchGameAtStart) m_StateMachine.Play<LaunchGameState>();
        }
    }
}