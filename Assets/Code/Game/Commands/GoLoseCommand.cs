using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoLoseCommand : MonoBehaviour, ICommand
{
    private LoseWidget m_LoseWidget;
    private CommandSystem m_CommandSystem;
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        UISystem.Instance.compositionModule.Show<UILoseComposion>();
        m_LoseWidget = UISystem.Instance.GetElement<LoseWidget>();
        m_LoseWidget.OnRetry += OnRetry;
        Debug.Log("Lose!");
    }

    private void OnRetry()
    {
        m_LoseWidget.OnRetry -= OnRetry;
        m_CommandSystem.Execute<GoHomeCommand>();
        m_LoseWidget = null;
        m_CommandSystem = null;
    }
}
