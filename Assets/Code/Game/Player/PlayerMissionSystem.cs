using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissionSystem : MonoBehaviour, IEcsInitSystem, IEcsGroup<Update>
{
    private PlayerPrefsData<int> m_Level;

    public int level => m_Level.Value;

    public void CompleteLevel()
    {
        m_Level.Value++;
    }

    public void Init(IEcsSystems systems)
    {
        m_Level = new PlayerPrefsData<int>(nameof(m_Level), 1);
    }
}
