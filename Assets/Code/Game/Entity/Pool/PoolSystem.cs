using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour, IEcsInitSystem, IEcsGroup<Update>
{
    private VFXPoolProvider[] m_Pools;

    public void Init(IEcsSystems systems)
    {
        m_Pools = GetComponentsInChildren<VFXPoolProvider>();
    }
    public T GetPool<T>() where T : VFXPoolProvider
    {
        for (int i = 0; i < m_Pools.Length; i++)
        {
            if (m_Pools[i] is T) return (T)m_Pools[i];
        }
        return null;
    }
}
