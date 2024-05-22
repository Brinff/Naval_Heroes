using Code.Game.Ads;
using Code.Game.Analytics;
using Code.Game.Wallet;
using Code.Services;
using Code.States;
using Game.UI;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Game.States
{
    public class WinState : MonoBehaviour, IPlayState, IStopState, IUpdateState
    {
        private WinWidget m_WinWidget;
        private CommandSystem m_CommandSystem;
        private PlayerMissionSystem m_PlayerLevelSystem;
        private WalletService m_WalletService;
        private int m_Reward;
        private BattleData m_BattleData;

        [SerializeField]
        private float m_SpeedCleon;
        private float m_PositionCleon;
        private int m_RewardCleon;

        [SerializeField] private GameObject m_FinalVirtualCamera;

        public void SetBattleData(BattleData battleData)
        {
            m_BattleData = battleData;
        }

        public void OnPlay(IStateMachine stateMachine)
        {
            var entityManager = ServiceLocator.Get<EntityManager>();

            m_CommandSystem = entityManager.GetSystem<CommandSystem>();
            m_PlayerLevelSystem = entityManager.GetSystem<PlayerMissionSystem>();

            m_FinalVirtualCamera.SetActive(true);

            //SmartlookUnity.Smartlook.TrackNavigationEvent("Battle", SmartlookUnity.Smartlook.NavigationEventType.exit);
            TinySauce.OnGameFinished(true, 0, m_BattleData.level);

            m_PlayerLevelSystem.CompleteLevel();
            m_Reward = m_BattleData.winReward;

            var usService = ServiceLocator.Get<UIRoot>();

            m_WinWidget = usService.GetWidget<WinWidget>();
            m_PositionCleon = 0;
            m_ActiveCleon = true;

            m_WinWidget.OnClaim += OnClaim;
            m_WinWidget.OnNoThanks += OnNoThanks;
            m_WinWidget.rewardLabel.SetValue(m_Reward, true);
            m_WinWidget.missionLabel.SetValue(m_BattleData.level, true);
            m_WinWidget.cleon.Evaluate(m_PositionCleon);


            ServiceLocator.Get<WalletService>().IncomeValue(m_Reward, "Game", "Win");

            ServiceLocator.Get<UICompositionController>().Show<UIWinCompositon>();
        }


        private bool m_ActiveCleon;

        private void OnClaim()
        {

            //m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);

            if (ServiceLocator.Get<AdsCleonReward>().Show(OnClaimReward))
            {
                m_ActiveCleon = false;
            }
        }

        private void OnClaimReward(bool done)
        {
            if (done)
            {
                m_CommandSystem.Execute<EndBattleCommand>();
                ServiceLocator.Get<GameStateMachine>().Play<HomeState>();
                ServiceLocator.Get<WalletService>().IncomeValue(m_RewardCleon - m_Reward, AnalyticService.ADS, "Win");
            }
            else
            {
                m_ActiveCleon = true;
            }
        }

        private void OnNoThanks()
        {
            
            //m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
            m_CommandSystem.Execute<EndBattleCommand>();
            ServiceLocator.Get<AdsBattleInterstitial>().Show();
            ServiceLocator.Get<GameStateMachine>().Play<HomeState>();
        }


        public void OnStop(IStateMachine stateMachine)
        {
            m_WinWidget.OnClaim -= OnClaim;
            m_WinWidget.OnNoThanks -= OnNoThanks;
            m_FinalVirtualCamera.SetActive(false);
        }

        public void OnUpdate(IStateMachine stateMachine)
        {
            if (m_ActiveCleon)
            {
                var value = m_WinWidget.cleon.Evaluate(m_PositionCleon);
                m_PositionCleon += Time.unscaledDeltaTime * m_SpeedCleon;
                m_RewardCleon = Mathf.RoundToInt(value * m_Reward);
                m_WinWidget.cleonRewardLabel.DoValue(m_RewardCleon - m_Reward, 0.1f);
            }
        }
    }
}