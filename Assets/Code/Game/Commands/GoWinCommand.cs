using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Game.States;
using Code.Game.Wallet;
using Code.Services;
using UnityEngine;

public class GoWinCommand : MonoBehaviour, ICommand<BattleData>
{
    private WinWidget m_WinWidget;
    private CommandSystem m_CommandSystem;
    private PlayerMissionSystem m_PlayerLevelPovider;
    private WalletService m_WalletService;
    private int m_Reward;

    private bool m_IsLockClaim;
    private IEnumerator WaitLockClaim()
    {
        m_IsLockClaim = true;
        yield return new WaitForSeconds(GameSettings.Instance.timeLockScreen);
        m_IsLockClaim = false;
    }

    public void Execute(EcsWorld world, IEcsSystems systems, BattleData battleData)
    {
        StartCoroutine(WaitLockClaim());
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_PlayerLevelPovider = systems.GetSystem<PlayerMissionSystem>();

        TinySauce.OnGameFinished(true, 0, battleData.level);

        m_PlayerLevelPovider.CompleteLevel();
        m_Reward = battleData.winReward;

        //m_PlayerMoneyProvider = systems.GetSystem<PlayerMoneySystem>();

        var usService = ServiceLocator.Get<UIController>();
        
        m_WinWidget = usService.GetElement<WinWidget>();
        m_WinWidget.OnClaim += OnClaimReward;
/*        m_WinWidget.SetReward(battleData.winReward, false);
        m_WinWidget.SetLevel(battleData.level);*/
        usService.compositionModule.Show<UIWinCompositon>();

        ServiceLocator.Get<WalletService>().IncomeValue(m_Reward, "Game", "Win");
    }

    private void OnClaimReward()
    {
        if (m_IsLockClaim) return;

        m_WinWidget.OnClaim -= OnClaimReward;
        
        //m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
        m_CommandSystem.Execute<EndBattleCommand>();
        ServiceLocator.Get<GameStateMachine>().Play<HomeState>();

        m_WalletService = null;
        m_WinWidget = null;
        m_PlayerLevelPovider = null;
        m_CommandSystem = null;
    }
}
