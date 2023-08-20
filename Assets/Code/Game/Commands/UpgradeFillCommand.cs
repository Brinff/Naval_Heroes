using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UpgradeFillCommand : MonoBehaviour, ICommand
{
    private CommandSystem m_CommandSystem;
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        var upgradeWidget = UISystem.Instance.GetElement<UpgradeWidget>();
        if (!upgradeWidget.isFilled)
        {
            upgradeWidget.OnUpgrade += OnUpgrade;
            var playerUpgradeData = systems.GetData<UpgradeDatabase>();
            var playerUpgradeProvider = systems.GetSystem<PlayerUpgradeSystem>();
            upgradeWidget.Fill(playerUpgradeData.upgrades, playerUpgradeProvider.upgrades);
        }
    }

    private void OnUpgrade(UpgradeData upgradeData)
    {
        m_CommandSystem.Execute<UpgradeBuyCommand, UpgradeData>(upgradeData);
    }
}
