using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEditor.Rendering.Universal;

public class PlayerAimSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<TurretAimComponent> m_PoolTurretAimComponent;
    private EcsPool<PlayerAimPointComponent> m_PoolAimPoint;
    private EcsPool<Root> m_PoolRootComponent;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<LookAtViewComponent> m_PoolLookAtView;
    private EcsPool<EyeRaycastComponent> m_PoolEyeRaycast;
    private EcsWorld m_World;


    private EcsFilter m_PlayerFilter;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<TurretAimComponent>().Inc<PlayerTag>().End();

        m_PlayerFilter = m_World.Filter<PlayerAimPointComponent>().Inc<LookAtViewComponent>().Inc<PlayerTag>().Exc<AITag>().End();

        m_PoolTurretAimComponent = m_World.GetPool<TurretAimComponent>();
        m_PoolRootComponent = m_World.GetPool<Root>();
        m_PoolAimPoint = m_World.GetPool<PlayerAimPointComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolLookAtView = m_World.GetPool<LookAtViewComponent>();
        m_PoolEyeRaycast = m_World.GetPool<EyeRaycastComponent>();
    }


    private void OnEnable()
    {
        
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_PlayerFilter)
        {
            ref var lookAtView = ref m_PoolLookAtView.Get(entity);
            if (lookAtView.eyeEntity.Unpack(m_World, out int eyeEntity))
            {
                ref var eyeRaycast = ref m_PoolEyeRaycast.Get(eyeEntity);
                ref var aimPoint = ref m_PoolAimPoint.Get(entity);
                aimPoint.position = eyeRaycast.point;
            }
        }

        foreach (var entity in m_Filter)
        {
            ref var turretAimComponent = ref m_PoolTurretAimComponent.Get(entity);

            ref var rootComponent = ref m_PoolRootComponent.Get(entity);

            bool hasAim = rootComponent.entity.Unpack(m_World, out int rootEntity) && m_PoolAimPoint.Has(rootEntity);

            if (hasAim)
            {
                ref var aimPoint = ref m_PoolAimPoint.Get(rootEntity);
                turretAimComponent.target = aimPoint.position;
                turretAimComponent.state = aimPoint.state;
            }
            else
            {
                ref var transform = ref m_PoolTransform.Get(entity);
                turretAimComponent.state = AimState.Idle;
            }
        }
    }
}
