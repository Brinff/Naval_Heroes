using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoWinCommand : MonoBehaviour, ICommand<BattleData>
{
    private WinWidget m_WinWidget;
    private CommandSystem m_CommandSystem;
    private PlayerMissionSystem m_PlayerLevelPovider;
    private PlayerMoneySystem m_PlayerMoneyProvider;
    private int m_Reward;

    public void Execute(EcsWorld world, IEcsSystems systems, BattleData battleData)
    {
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_PlayerLevelPovider = systems.GetSystem<PlayerMissionSystem>();

        m_PlayerLevelPovider.CompleteLevel();
        m_Reward = battleData.winReward;

        //m_PlayerMoneyProvider = systems.GetSystem<PlayerMoneySystem>();

        m_WinWidget = UISystem.Instance.GetElement<WinWidget>();
        m_WinWidget.OnClaim += OnClaimReward;
        m_WinWidget.SetReward(battleData.winReward, false);
        m_WinWidget.SetLevel(battleData.level);
        UISystem.Instance.compositionModule.Show<UIWinCompositon>();
    }

    private void OnClaimReward()
    {
        m_WinWidget.OnClaim -= OnClaimReward;
        m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
        m_CommandSystem.Execute<GoHomeCommand>();

        m_PlayerMoneyProvider = null;
        m_WinWidget = null;
        m_PlayerLevelPovider = null;
        m_CommandSystem = null;
    }
}
