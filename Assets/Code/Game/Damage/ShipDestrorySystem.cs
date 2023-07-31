using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShipDestrorySystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<DestroyComponent> m_DestroyEntity;
    private EcsPool<TransformComponent> m_PoolTransform;
    [SerializeField]
    private float m_DelayDestory;
    [SerializeField]
    private float m_OffsetExplosion;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<HealthEndEvent>().Inc<ShipTag>().Inc<TransformComponent>().End();
        m_DestroyEntity = m_World.GetPool<DestroyComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var destoryEntity = ref m_DestroyEntity.Add(entity);
            ref var transform = ref m_PoolTransform.Get(entity);
            systems.GetShared<SharedData>().Get<VFXShipExplosion>().Play(transform.transform.position + Vector3.up * m_OffsetExplosion, Quaternion.identity);
            destoryEntity.delay = m_DelayDestory;
        }
    }
}
