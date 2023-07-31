using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelProvider : MonoBehaviour, ISharedData, ISharedInitalizeData
{
    private PlayerPrefsData<int> m_Level;

    public void InitalizeData()
    {
        m_Level = new PlayerPrefsData<int>(nameof(m_Level), 1);
    }

    public int level => m_Level.Value;

    public void CompleteLevel()
    {
        m_Level.Value++;
    }
}
