using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUpdateCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        SharedData sharedData = systems.GetShared<SharedData>();

        var playerUpgradeData = sharedData.Get<PlayerUpgradeData>();
        var playerUpgradeProvider = sharedData.Get<PlayerUpgradeProvider>();
        var moneySoftProvider = sharedData.Get<PlayerMoneySoftProvider>();
        
        var upgradeWidget = UISystem.Instance.GetElement<UpgradeWidget>();
        var playerUpgrades = playerUpgradeProvider.upgrades;
        for (int i = 0; i < Mathf.Min(playerUpgradeData.upgrades.Count, playerUpgrades.Count); i++)
        {
            var upgrade = playerUpgradeData.upgrades[i];
            var playerUpgrade = playerUpgrades[i];
            upgradeWidget.SetHasMoney(upgrade, moneySoftProvider.HasMoney(upgrade.cost[playerUpgrade.level - 1]));
        }
    }
}
