using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Game.Wallet;
using Code.Services;
using UnityEngine;

public class UpgradeUpdateCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var upgradeData = systems.GetData<UpgradeDatabase>();
        var playerUpgradeProvider = systems.GetSystem<PlayerUpgradeSystem>();
        var walletService = ServiceLocator.Get<WalletService>();
        
        var upgradeWidget = ServiceLocator.Get<UIService>().GetElement<UpgradeWidget>();
        var playerUpgrades = playerUpgradeProvider.upgrades;
        for (int i = 0; i < Mathf.Min(upgradeData.upgrades.Count, playerUpgrades.Count); i++)
        {
            var upgrade = upgradeData.upgrades[i];
            var playerUpgrade = playerUpgrades[i];
            upgradeWidget.SetHasMoney(upgrade, walletService.IsEnough(upgrade.cost[playerUpgrade.level - 1]));
        }
    }
}
