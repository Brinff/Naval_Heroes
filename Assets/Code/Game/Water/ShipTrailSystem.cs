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
    [SerializeField]
    private float m_WaterHeight = 0;
    [SerializeField]
    private Quaternion m_VFXTrailRotation = Quaternion.identity;
    [SerializeField]
    private bool m_Debug = true;

    private EcsWorld m_World;

    private IObjectPool<ParticleSystem> m_Pool;

    private EcsFilter m_ShipTrailFilter;
    private EcsFilter m_ShipTrailDeadFilter;
    private EcsFilter m_ShipTrailReleaseFilter;
    private EcsPool<ShipTrail> m_PoolShipTrail;

    public void Init(IEcsSystems systems)
    {
        m_VFXTrailParticleSystem = m_VFXTrail.GetComponent<ParticleSystem>();
        m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, m_VFXTrailPoolSize);

        m_World = systems.GetWorld();
        m_ShipTrailFilter = m_World.Filter<ShipTrail>().Exc<DeadTag>().End();
        m_PoolShipTrail = m_World.GetPool<ShipTrail>();
        m_ShipTrailDeadFilter = m_World.Filter<ShipTrail>().Inc<DeadTag>().End();
        m_ShipTrailReleaseFilter = m_World.Filter<ShipTrail>().Inc<Release>().End();
    }

    private ParticleSystem CreatePooledItem()
    {
        return Instantiate(m_VFXTrailParticleSystem, transform, false);
    }

    private void OnReturnedToPool(ParticleSystem system)
    {
        system.SetEmission(false);
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

        foreach (var entity in m_ShipTrailFilter)
        {
            ref var shipTrail = ref m_PoolShipTrail.Get(entity);
            Vector3 position = shipTrail.orgin.position;
            position.y = m_WaterHeight;

            if (shipTrail.particleSystem == null)
            {
                shipTrail.particleSystem = m_Pool.Get();

                shipTrail.particleSystem.transform.rotation = m_VFXTrailRotation;
                shipTrail.particleSystem.transform.localScale = shipTrail.scale;
                var main = shipTrail.particleSystem.main;
                main.startLifetime = shipTrail.lifetime;
                shipTrail.particleSystem.gameObject.SetActive(true);
                shipTrail.particleSystem.SetEmission(true);
                shipTrail.particleSystem.Play();
            }
            shipTrail.particleSystem.transform.position = position;

            if (m_Debug)
            {
                shipTrail.particleSystem.transform.rotation = m_VFXTrailRotation;
                shipTrail.particleSystem.transform.localScale = shipTrail.scale;
                var main = shipTrail.particleSystem.main;
                main.startLifetime = shipTrail.lifetime;
            }
        }

        foreach (var entity in m_ShipTrailDeadFilter)
        {
            ref var shipTrail = ref m_PoolShipTrail.Get(entity);
            if (shipTrail.particleSystem)
            {
                m_Pool.Release(shipTrail.particleSystem);
                shipTrail.particleSystem = null;
            }
        }

        foreach (var entity in m_ShipTrailReleaseFilter)
        {
            ref var shipTrail = ref m_PoolShipTrail.Get(entity);
            if (shipTrail.particleSystem)
            {
                Debug.Log("Return VFX");
                m_Pool.Release(shipTrail.particleSystem);
                shipTrail.particleSystem.Stop();
                shipTrail.particleSystem.gameObject.SetActive(false);
                shipTrail.particleSystem = null;
            }
        }
    }
}
