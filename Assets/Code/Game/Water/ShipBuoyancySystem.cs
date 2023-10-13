using Leopotam.EcsLite;
using OceanSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuoyancySystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroup<Update>
{
    [SerializeField]
    private OceanSimulation m_OceanSimulation;

    private EcsWorld m_World;
    private EcsPool<ShipBuoyancy> m_PoolShipBuoyancy;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsFilter m_Filter;

    [SerializeField]
    private Vector3 m_Rotation;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_PoolShipBuoyancy = m_World.GetPool<ShipBuoyancy>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_Filter = m_World.Filter<ShipBuoyancy>().Inc<TransformComponent>().Exc<DeadTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var shipTransform = ref m_PoolTransform.Get(entity);
            ref var shipBuoyancy = ref m_PoolShipBuoyancy.Get(entity);
            var shipPosition = shipTransform.transform.position;
            if(m_OceanSimulation.Collision.GetWaterData(shipPosition, out Vector3 waterPosition, out Vector3 waterNormal))
            {
                shipTransform.transform.position = waterPosition - Vector3.up * shipBuoyancy.waterLine;
                //Debug.Log(waterNormal)
                waterNormal += Vector3.up * 5;
                waterNormal = Vector3.Normalize(waterNormal);

                shipTransform.transform.rotation = Quaternion.LookRotation(Vector3.forward, waterNormal);
            }
        }
    }
}
