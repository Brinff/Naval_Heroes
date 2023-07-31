using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeProvider : MonoBehaviour, ISharedData, ISharedInitalizeData
{
    private PlayerPrefsData<List<UpgradePlayerData>> m_PlayerUpgrades;

    public void InitalizeData()
    {
        m_PlayerUpgrades = new PlayerPrefsData<List<UpgradePlayerData>>(nameof(m_PlayerUpgrades), new List<UpgradePlayerData>(3) { new UpgradePlayerData(), new UpgradePlayerData(), new UpgradePlayerData() });
    }

    public List<UpgradePlayerData> upgrades => m_PlayerUpgrades.Value;

    public UpgradePlayerData GetUpgrade(int index)
    {
        return m_PlayerUpgrades.Value[index];
    }

    public void AddLevel(int index)
    {
        m_PlayerUpgrades.Value[index].level++;
        m_PlayerUpgrades.Save();
    }

    public bool IsMaxLevel(int index, int maxLevel)
    {
        return m_PlayerUpgrades.Value[index].level >= maxLevel;
    }
}
