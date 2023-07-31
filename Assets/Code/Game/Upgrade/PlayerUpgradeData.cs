using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeData : MonoBehaviour, ISharedData
{
    [SerializeField]
    private List<UpgradeData> m_Upgrades = new List<UpgradeData>();

    public List<UpgradeData> upgrades => m_Upgrades;
}
