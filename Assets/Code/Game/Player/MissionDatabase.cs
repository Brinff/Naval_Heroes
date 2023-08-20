using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionDatabase : MonoBehaviour, IEcsData
{
    [SerializeField]
    private List<LevelData> m_Levels = new List<LevelData>();

    public LevelData GetLevel(int level)
    {
        int index = (int)Mathf.Repeat(level - 1, m_Levels.Count);
        return m_Levels[index];
    }
}
