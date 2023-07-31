using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class AIMoveToTargetSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    [SerializeField]
    private float m_Radius = 40;
    [SerializeField]
    private float m_RudderFactor = 2;
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<AIMoveToTargetComponent> m_PoolAIMoveToTarget;
    private EcsPool<AITargetComponent> m_PoolAITarget;
    private EcsPool<PropulsionComponent> m_PoolPropulsion;
    private EcsPool<TransformComponent> m_PoolTransform;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<AIMoveToTargetComponent>().Inc<AITargetComponent>().Inc<TransformComponent>().Inc<PropulsionComponent>().End();
        m_PoolAIMoveToTarget = m_World.GetPool<AIMoveToTargetComponent>();
        m_PoolAITarget = m_World.GetPool<AITargetComponent>();
        m_PoolPropulsion = m_World.GetPool<PropulsionComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var aiTarget = ref m_PoolAITarget.Get(entity);
            if (aiTarget.entity.Unpack(m_World, out int targetEntity))
            {
                ref var targetTransform = ref m_PoolTransform.Get(targetEntity);
                ref var transform = ref m_PoolTransform.Get(entity);
                var delta = targetTransform.transform.position - transform.transform.position;
                var direction = Vector3.Normalize(delta);
                Vector3 rightDirection = transform.transform.right;
                rightDirection.y = 0;
                Vector3 forwardDirection = transform.transform.forward;
                forwardDirection.y = 0;
                direction.y = 0;

                ref var propulsion = ref m_PoolPropulsion.Get(entity);

                float throtle = delta.magnitude < m_Radius ? -propulsion.rigidbody.velocity.magnitude / m_Radius : delta.magnitude / (propulsion.rigidbody.velocity.magnitude + m_Radius);

                var dotRight = Vector3.Dot(rightDirection, direction);
                var dotForward = Vector3.Dot(forwardDirection, direction);
                if (dotForward < 0)
                {
                    dotRight = Mathf.Sign(dotRight);
                }

                propulsion.throttle = Mathf.Clamp(throtle, -1, 1);
                propulsion.rudder = Mathf.Clamp(dotRight * m_RudderFactor, -1, 1);
            }
        }
    }

    //    m_PointOnBounds = m_PropulsionController.rigidbody.ClosestPointOnBounds(m_Target);
    //        m_Delta = m_Target - m_PointOnBounds;
    //        m_Direction = Vector3.Normalize(m_Delta);
    //        Vector3 rightDirection = m_PropulsionController.rigidbody.transform.right;
    //    rightDirection.y = 0;

    //        Vector3 forwardDirection = m_PropulsionController.rigidbody.transform.forward;
    //    forwardDirection.y = 0;

    //        m_Direction.y = 0;

    //        float throtle = m_Delta.magnitude < m_Radius ? -m_PropulsionController.rigidbody.velocity.magnitude / m_Radius : m_Delta.magnitude / (m_PropulsionController.rigidbody.velocity.magnitude + m_Radius);

    //    m_DotRight = Vector3.Dot(rightDirection, m_Direction);
    //        m_DotForward = Vector3.Dot(forwardDirection, m_Direction);
    //        if (m_DotForward< 0)
    //        {
    //            m_DotRight = Mathf.Sign(m_DotRight);
    //        }
    //m_PropulsionController.RudderAngle = Mathf.Clamp(m_DotRight * 2, -1, 1);
    //m_PropulsionController.Throttle = throtle;
}
