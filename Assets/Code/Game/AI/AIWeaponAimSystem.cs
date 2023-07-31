using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponAimSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    [SerializeField]
    private float m_AimRange;
    private EcsFilter m_Filter;
    private EcsPool<TurretAimComponent> m_PoolWeaponAim;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<AITargetComponent> m_PoolAITarget;
    private EcsPool<AimBoundsComponent> m_PoolAimBounds;

    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<AITag>().Inc<AITargetComponent>().Inc<TurretAimComponent>().Inc<TransformComponent>().End();
        m_PoolWeaponAim = m_World.GetPool<TurretAimComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolAITarget = m_World.GetPool<AITargetComponent>();
        m_PoolAimBounds = m_World.GetPool<AimBoundsComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var aiTarget = ref m_PoolAITarget.Get(entity);
            if (aiTarget.entity.Unpack(m_World, out int targetEntity))
            {
                ref var targetEntityTransform = ref m_PoolAimBounds.Get(targetEntity);
                ref var weaponAim = ref m_PoolWeaponAim.Get(entity);
                ref var transform = ref m_PoolTransform.Get(entity);

                weaponAim.state = AimState.Aim;

                float distanceToTarget = Vector3.Distance(transform.transform.position, targetEntityTransform.center);

                if (distanceToTarget < m_AimRange)
                {
                    weaponAim.target = targetEntityTransform.center;
                }
            }
            else
            {
                ref var weaponAim = ref m_PoolWeaponAim.Get(entity);
                weaponAim.state = AimState.Idle;
            }
        }
    }
}
