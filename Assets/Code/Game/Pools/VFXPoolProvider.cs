using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class VFXPoolProvider : MonoBehaviour
{
    public enum PoolType
    {
        Stack,
        LinkedList
    }

    [SerializeField]
    private PoolType m_PoolType;
    [SerializeField]
    private bool m_Checks = true;
    [SerializeField]
    private int m_MaxPoolSize = 10;

    [SerializeField]
    private GameObject m_Prefab = null;
    [SerializeField]
    private float m_Lifetime = 1;

    private IObjectPool<GameObject> m_Pool;

    public struct Timer
    {
        public GameObject visualEffect;
        public float time;
    }

    private Timer[] m_Timers;
    private Stack<int> m_Unused;

    [Button]
    public void Create()
    {
        m_Timers = new Timer[100];
        m_Unused = new Stack<int>(100);
        for (int i = 0; i < 100; i++)
        {
            m_Unused.Push(i);
        }

        if (m_Pool == null && m_Prefab)
        {
            if (m_PoolType == PoolType.Stack) m_Pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_Checks, 10, m_MaxPoolSize);
            if (m_PoolType == PoolType.LinkedList) m_Pool = new LinkedPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_Checks, m_MaxPoolSize);
        }
    }

    private void Update()
    {
        if (m_Timers == null) return;

        int count = m_Timers.Length;
        for (int i = 0; i < count; i++)
        {
            ref var timer = ref m_Timers[i];
            if (timer.visualEffect != null)
            {
                if (timer.time <= 0)
                {
                    m_Pool.Release(timer.visualEffect);
                    m_Unused.Push(i);
                    timer.visualEffect = null;
                }
                else timer.time -= Time.deltaTime;
            }
        }
    }

    [Button]
    public GameObject Play(Vector3 position, Quaternion rotation)
    {
        if (m_Pool != null)
        {
            GameObject visualEffect = m_Pool.Get();
            visualEffect.transform.position = position;
            visualEffect.transform.rotation = rotation;
            int index = m_Unused.Pop();
            ref var timer = ref m_Timers[index];
            timer.visualEffect = visualEffect;
            timer.time = m_Lifetime;
            return visualEffect;
        }
        return null;
    }


    private GameObject CreatePooledItem()
    {
        return Instantiate(m_Prefab, transform, false);
    }

    private void OnReturnedToPool(GameObject system)
    {
        system.transform.SetParent(transform);
        system.SetActive(false);
    }

    private void OnTakeFromPool(GameObject system)
    {
        system.SetActive(true);
    }

    private void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system);
    }

}
