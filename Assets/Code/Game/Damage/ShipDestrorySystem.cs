using DG.Tweening;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class ShipDestrorySystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    //private EcsPool<DestroyComponent> m_DestroyEntity;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<BuoyancySimulate> m_PoolBuoyancySimulate;
    [SerializeField]
    private float m_DelayDestory;
    [SerializeField]
    private float m_OffsetExplosion;

    private EcsFilter m_BuoyancyFilter;
    [SerializeField]
    private AnimationCurve m_CurvePosition;
    [SerializeField]
    private AnimationCurve m_CurveRotation;

    public float speedDown;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<HealthEndEvent>().Inc<ShipTag>().Inc<TransformComponent>().Exc<BuoyancySimulate>().End();
        //m_DestroyEntity = m_World.GetPool<DestroyComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolBuoyancySimulate = m_World.GetPool<BuoyancySimulate>();
        m_BuoyancyFilter = m_World.Filter<BuoyancySimulate>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            //ref var destoryEntity = ref m_DestroyEntity.Add(entity);
            ref var transform = ref m_PoolTransform.Get(entity);
            systems.GetSystem<PoolSystem>().GetPool<VFXShipExplosion>().Play(transform.transform.position + Vector3.up * m_OffsetExplosion, Quaternion.identity);
            ref var buoyancySimulate = ref m_PoolBuoyancySimulate.Add(entity);

            buoyancySimulate.position = transform.transform.position;
            buoyancySimulate.rotation = transform.transform.rotation;

            buoyancySimulate.targetPosition = transform.transform.position - Vector3.up * 100f;
            buoyancySimulate.targetRotation = Quaternion.Euler(Random.Range(-60, 60), Random.Range(-5, 5), Random.Range(-60, 60));
            //destoryEntity.delay = m_DelayDestory;
        }

        foreach (var entity in m_BuoyancyFilter)
        {
            ref var transform = ref m_PoolTransform.Get(entity);
            ref var buoyancySimulate = ref m_PoolBuoyancySimulate.Get(entity);

            transform.transform.position = Vector3.LerpUnclamped(buoyancySimulate.position, buoyancySimulate.targetPosition, m_CurvePosition.Evaluate(buoyancySimulate.t));
            transform.transform.rotation = Quaternion.LerpUnclamped(buoyancySimulate.rotation, buoyancySimulate.targetRotation, m_CurveRotation.Evaluate(buoyancySimulate.t));

            buoyancySimulate.t += Time.deltaTime * speedDown;
        }
    }
}
