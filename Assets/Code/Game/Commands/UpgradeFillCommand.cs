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
            var sharedData = systems.GetShared<SharedData>();
            var playerUpgradeData = sharedData.Get<PlayerUpgradeData>();
            var playerUpgradeProvider = sharedData.Get<PlayerUpgradeProvider>();
            upgradeWidget.Fill(playerUpgradeData.upgrades, playerUpgradeProvider.upgrades);
        }
    }

    private void OnUpgrade(UpgradeData upgradeData)
    {
        m_CommandSystem.Execute<UpgradeBuyCommand, UpgradeData>(upgradeData);
    }
}
