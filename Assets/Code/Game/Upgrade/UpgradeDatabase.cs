using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDatabase : MonoBehaviour, IEcsData
{
    [SerializeField]
    private List<UpgradeData> m_Upgrades = new List<UpgradeData>();

    public List<UpgradeData> upgrades => m_Upgrades;
}
