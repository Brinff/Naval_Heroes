using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private UpgradeItemWidget m_Prefab;
    [SerializeField]
    private Transform m_Root;

    private List<UpgradeItemWidget> m_Items = new List<UpgradeItemWidget>();
    public bool isFilled => m_Items.Count > 0;

    public void Fill(List<UpgradeData> upgrades, List<UpgradePlayerData> upgradePlayer)
    {
        m_Prefab.gameObject.SetActive(false);
        for (int i = 0; i < Mathf.Min(upgrades.Count, upgradePlayer.Count); i++)
        {
            CreateItem(upgrades[i], upgradePlayer[i]);
        }
    }

    private void CreateItem(UpgradeData upgrade, UpgradePlayerData upgradePlayer)
    {
        var instance = Instantiate(m_Prefab);
        instance.gameObject.SetActive(true);
        instance.transform.SetParent(m_Root);
        instance.SetData(upgrade);
        instance.SetIcon(upgrade.sprite);
        instance.SetClickHandler(this);
        instance.SetTitle(upgrade.title);
        instance.SetLevel(upgradePlayer.level, true);
        instance.SetCost(upgrade.cost[upgradePlayer.level - 1], true);

        m_Items.Add(instance);
    }

    public void Upgrade(UpgradeData upgrade, UpgradePlayerData upgradePlayer)
    {
        var item = m_Items.Find(x => x.ContainsData(upgrade));
        item.PlayUpgrade();
        item.SetLevel(upgradePlayer.level, false);
        item.SetCost(upgrade.cost[upgradePlayer.level - 1], false);
    }

    public void PlayNoMoney(UpgradeData upgradeData)
    {
        var item = m_Items.Find(x => x.ContainsData(upgradeData));
        item.PlayNoMoney();
    }

    

    public void Clear()
    {

    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public delegate void UpgradeDelegate(UpgradeData upgradeData);

    public event UpgradeDelegate OnUpgrade;

    public void OnClickUpgrade(UpgradeItemWidget upgradeItemWidget, UpgradeData upgradeData)
    {
        OnUpgrade?.Invoke(upgradeData);
    }

    public void SetHasMoney(UpgradeData upgradeData, bool isHasMoney)
    {
        var item = m_Items.Find(x => x.ContainsData(upgradeData));
        item.SetHasMoney(isHasMoney);
    }
}
