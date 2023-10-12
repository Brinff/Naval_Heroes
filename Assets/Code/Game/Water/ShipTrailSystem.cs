using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShipTrailSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    [SerializeField]
    private GameObject m_VFXTrail;
    private ParticleSystem m_VFXTrailParticleSystem;
    [SerializeField]
    private int m_VFXTrailPoolSize = 10;

    private IObjectPool<ParticleSystem> m_Pool;

    public void Init(IEcsSystems systems)
    {
        m_VFXTrailParticleSystem = m_VFXTrail.GetComponent<ParticleSystem>();
        m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, m_VFXTrailPoolSize);
    }

    private ParticleSystem CreatePooledItem()
    {
        return Instantiate(m_VFXTrailParticleSystem, transform, false);
    }

    private void OnReturnedToPool(ParticleSystem system)
    {
        system.transform.SetParent(transform);
        system.Stop();
    }

    private void OnTakeFromPool(ParticleSystem system)
    {
        system.Play();
    }

    private void OnDestroyPoolObject(ParticleSystem system)
    {
        Destroy(system.gameObject);
    }

    public void Run(IEcsSystems systems)
    {
        
    }
}
