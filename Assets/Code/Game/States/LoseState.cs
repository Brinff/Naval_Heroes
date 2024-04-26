using Assets.Code.Game.Ads;
using Code.Game.Wallet;
using Code.Services;
using Code.States;
using Game.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.States
{
    public class LoseState : MonoBehaviour,IPlayState
    {
        private LoseWidget m_LoseWidget;
        private CommandSystem m_CommandSystem;
        private int m_Reward;

        private BattleData m_BattleData;
        
        public void SetBattleData(BattleData battleData)
        {
            m_BattleData = battleData;
        }
        
        public void OnPlay(IStateMachine stateMachine)
        {
            var entityManager = ServiceLocator.Get<EntityManager>();
            var uiService = ServiceLocator.Get<UIController>();
        
            m_CommandSystem = entityManager.GetSystem<CommandSystem>();
            uiService.compositionModule.Show<UILoseComposion>();
            m_LoseWidget = uiService.GetElement<LoseWidget>();
            m_LoseWidget.SetReward(m_Reward = m_BattleData.loseReward);
            m_LoseWidget.OnRetry += OnRetry;

            //SmartlookUnity.Smartlook.TrackNavigationEvent("Battle", SmartlookUnity.Smartlook.NavigationEventType.exit);
            TinySauce.OnGameFinished(false, 0, m_BattleData.level);
            Debug.Log("Lose!");
        }
        


        private void OnRetry()
        {
            m_LoseWidget.OnRetry -= OnRetry;
            ServiceLocator.Get<WalletService>().IncomeValue(m_Reward, "Game", "Lose");
            //m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
            m_CommandSystem.Execute<EndBattleCommand>();

            ServiceLocator.Get<AdsBattleInterstitial>().Show();

            ServiceLocator.Get<GameStateMachine>().Play<HomeState>();
            //m_CommandSystem.Execute<GoHomeCommand>();
        }
    }
}