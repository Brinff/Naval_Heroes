using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLoseCommand : MonoBehaviour, ICommand<BattleData>
{
    private LoseWidget m_LoseWidget;
    private CommandSystem m_CommandSystem;
    private int m_Reward;
    public void Execute(EcsWorld world, IEcsSystems systems, BattleData battleData)
    {
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        UISystem.Instance.compositionModule.Show<UILoseComposion>();
        m_LoseWidget = UISystem.Instance.GetElement<LoseWidget>();
        m_LoseWidget.SetReward(m_Reward = battleData.loseReward);
        m_LoseWidget.OnRetry += OnRetry;

        SmartlookUnity.Smartlook.TrackNavigationEvent("Battle", SmartlookUnity.Smartlook.NavigationEventType.exit);
        TinySauce.OnGameFinished(false, 0, battleData.level);
        Debug.Log("Lose!");
    }

    private void OnRetry()
    {
        m_LoseWidget.OnRetry -= OnRetry;
        m_CommandSystem.Execute<MoneyAddCommand, int>(m_Reward);
        m_CommandSystem.Execute<GoHomeCommand>();
        m_LoseWidget = null;
        m_CommandSystem = null;
    }
}
