
using Code.Diagnostic;
using Code.Game.Wallet;
using Code.Services;
using Code.States;
using UnityEngine;

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
            
            ServiceLocator.ForEach<WalletService>(x => x.Initialize());
            ServiceLocator.ForEach<WalletMediator>(x => x.Initialize());
            
            ServiceLocator.ForEach<DiagnosticService>(x => x.Initialize());
            
            if (m_LaunchGameAtStart) m_StateMachine.Play<LaunchGameState>();
        }
    }
}