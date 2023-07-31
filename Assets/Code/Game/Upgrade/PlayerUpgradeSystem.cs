using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeSystem : MonoBehaviour/*, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem, IEcsDestroySystem*/
{


    //private UpgradeWidget m_UpgradeWidget;

    //private PlayerMoneySoftProvider m_MoneySoftProvider;

    //private EcsFilter m_Filter;
    //private CommandSystem m_CommandSystem;

    //public void Init(IEcsSystems systems)
    //{
    //    EcsWorld world = systems.GetWorld();
    //    m_Filter = world.Filter<GoHomeEvent>().End();
    //    m_CommandSystem = systems.GetSystem<CommandSystem>();

    //    m_MoneySoftProvider = systems.GetShared<SharedData>().Get<PlayerMoneySoftProvider>();

    //    m_UpgradeWidget = UISystem.Instance.GetElement<UpgradeWidget>();
    //    m_UpgradeWidget.OnUpgrade += OnUpgrade;

    //    m_UpgradeWidget.Fill(m_Upgrades, m_PlayerUpgrades);

    //    var playerUpgrades = m_PlayerUpgrades.Value;
    //    for (int i = 0; i < Mathf.Min(m_Upgrades.Count, playerUpgrades.Count); i++)
    //    {
    //        var upgrade = m_Upgrades[i];
    //        var playerUpgrade = playerUpgrades[i];
    //        m_UpgradeWidget.SetHasMoney(upgrade, m_MoneySoftProvider.HasMoney(upgrade.cost[playerUpgrade.level - 1]));
    //    }
    //}

    //private void OnUpgrade(UpgradeData upgradeData)
    //{
    //    int index = m_Upgrades.IndexOf(upgradeData);
    //    UpgradePlayerData upgradePlayerData = m_PlayerUpgrades.Value[index];
    //    int cost = upgradeData.cost[upgradePlayerData.level - 1];
    //    if (m_MoneySoftProvider.HasMoney(cost))
    //    {
    //        if (m_MoneySoftProvider.HasMoney(cost))
    //        {
    //            upgradePlayerData.level++;
    //            m_PlayerUpgrades.Save();

    //            m_CommandSystem.Execute<SpendMoneyCommand, int>(cost);

    //            m_UpgradeWidget.Upgrade(upgradeData, upgradePlayerData);
    //        }
    //    }
    //    else m_UpgradeWidget.PlayNoMoney(upgradeData);




    //    //else Debug.Log("No Money");
    //    //m_Upgrades.Find(x => x == upgradeData);
    //}

    //public void Run(IEcsSystems systems)
    //{
    //    foreach (var entity in m_Filter)
    //    {
    //        var playerUpgrades = m_PlayerUpgrades.Value;
    //        for (int i = 0; i < Mathf.Min(m_Upgrades.Count, playerUpgrades.Count); i++)
    //        {
    //            var upgrade = m_Upgrades[i];
    //            var playerUpgrade = playerUpgrades[i];
    //            m_UpgradeWidget.SetHasMoney(upgrade, m_MoneySoftProvider.HasMoney(upgrade.cost[playerUpgrade.level - 1]));
    //        }
    //    }
    //}

    //public void Destroy(IEcsSystems systems)
    //{
    //    if (m_UpgradeWidget != null) m_UpgradeWidget.OnUpgrade += OnUpgrade;
    //}
}
