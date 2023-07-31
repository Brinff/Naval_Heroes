using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class UpgradeBuyCommand : MonoBehaviour, ICommand<UpgradeData>
{
    [SerializeField]
    private ParticleSystem m_UpgradeEffect;
    public void Execute(EcsWorld world, IEcsSystems systems, UpgradeData upgradeData)
    {
        var commandSystem = systems.GetSystem<CommandSystem>();
        var sharedData = systems.GetShared<SharedData>();
        var playerUpgradeData = sharedData.Get<PlayerUpgradeData>();
        var playerUpgradeProvider = sharedData.Get<PlayerUpgradeProvider>();
        var playerMoneySoftProvider = sharedData.Get<PlayerMoneySoftProvider>();
        int index = playerUpgradeData.upgrades.IndexOf(upgradeData);
        UpgradePlayerData upgradePlayerData = playerUpgradeProvider.upgrades[index];
        int cost = upgradeData.cost[upgradePlayerData.level - 1];
        UpgradeWidget upgradeWidget = UISystem.Instance.GetElement<UpgradeWidget>();

        if (playerMoneySoftProvider.HasMoney(cost))
        {
            var playerShip = world.Filter<PlayerTag>().Inc<ShipTag>().Inc<TransformComponent>().End();
            var playerShipEntity = playerShip.GetSingleton();
            if (playerShipEntity != null)
            {
                ref var transform = ref world.GetPool<TransformComponent>().Get(playerShipEntity.Value);
                m_UpgradeEffect.transform.position = transform.transform.position;
                m_UpgradeEffect.transform.rotation = transform.transform.rotation;
                m_UpgradeEffect.Play();
            }

            playerUpgradeProvider.AddLevel(index);
            commandSystem.Execute<MoneySpendCommand, int>(cost);
            commandSystem.Execute<UpgradeUpdateCommand>();
            upgradeWidget.Upgrade(upgradeData, upgradePlayerData);
        }
        else upgradeWidget.PlayNoMoney(upgradeData);
    }
}
