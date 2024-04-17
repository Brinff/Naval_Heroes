using Code.Game.Wallet;
using Code.Services;
using Code.States;
using Game.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.States
{
    public class WinState : MonoBehaviour, IPlayState, IStopState
    {
        private WinWidget m_WinWidget;
        private CommandSystem m_CommandSystem;
        private PlayerMissionSystem m_PlayerLevelSystem;
        private WalletService m_WalletService;
        private int m_Reward;
        private BattleData m_BattleData;
        
        public void SetBattleData(BattleData battleData)
        {
            m_BattleData = battleData;
        }

        public void OnPlay(IStateMachine stateMachine)
        {
            var entityManager = ServiceLocator.Get<EntityManager>();
            
            m_CommandSystem = entityManager.GetSystem<CommandSystem>();
            m_PlayerLevelSystem = entityManager.GetSystem<PlayerMissionSystem>();

            //SmartlookUnity.Smartlook.TrackNavigationEvent("Battle", SmartlookUnity.Smartlook.NavigationEventType.exit);
            TinySauce.OnGameFinished(true, 0, m_BattleData.level);

            m_PlayerLevelSystem.CompleteLevel();
            m_Reward = m_BattleData.winReward;

            var usService = ServiceLocator.Get<UIController>();

            m_WinWidget = usService.GetElement<WinWidget>();
            m_WinWidget.OnClaim += OnClaimReward;
            m_WinWidget.SetReward(m_BattleData.winReward, false);
            m_WinWidget.SetLevel(m_BattleData.level);
            usService.compositionModule.Show<UIWinCompositon>();
        }
        
        

        private void OnClaimReward()
        {

            ServiceLocator.Get<WalletService>().IncomeValue(m_Reward, "Game", "Win");
            //m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
            m_CommandSystem.Execute<EndBattleCommand>();
            ServiceLocator.Get<GameStateMachine>().Play<HomeState>();
        }

        public void OnStop(IStateMachine stateMachine)
        {
            m_WinWidget.OnClaim -= OnClaimReward;
        }
    }
}