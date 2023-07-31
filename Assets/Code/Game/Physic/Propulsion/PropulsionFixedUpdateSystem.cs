using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Game;

public class PropulsionFixedUpdateSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupFixedUpdateSystem
{

    private EcsFilter m_Filter;
    private EcsPool<PropulsionComponent> m_PoolPropulsionComponent;
    private EcsPool<StatSpeedComponent> m_PoolStatSpeed;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        m_Filter = world.Filter<PropulsionComponent>().End();
        m_PoolPropulsionComponent = world.GetPool<PropulsionComponent>();
        m_PoolStatSpeed = world.GetPool<StatSpeedComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref PropulsionComponent propulsionComponent = ref m_PoolPropulsionComponent.Get(entity);

            if (m_PoolStatSpeed.Has(entity))
            {
                ref var statSpeed = ref m_PoolStatSpeed.Get(entity);
                propulsionComponent.maxSpeed = statSpeed.speed;
            }

            Rigidbody rigidbody = propulsionComponent.rigidbody;

            float maxSpeed = SpeedCovertor.Convert(propulsionComponent.maxSpeed, propulsionComponent.unit, SpeedUnit.M_PER_S);
            float targetSpeed = propulsionComponent.throttle * maxSpeed;
            float forwardAcceleration = SpeedCovertor.Convert(propulsionComponent.forwardAcceleration, propulsionComponent.unit, SpeedUnit.M_PER_S);
            float backwardAcceleration = SpeedCovertor.Convert(propulsionComponent.backwardAcceleration, propulsionComponent.unit, SpeedUnit.M_PER_S);

            Vector3 localVelocity = rigidbody.transform.InverseTransformVector(rigidbody.velocity);
            float delta = targetSpeed - localVelocity.z;

            if (delta > 0) delta *= forwardAcceleration;
            if (delta < 0) delta *= backwardAcceleration;

            targetSpeed += delta;

            Vector3 linearForce = Vector3.forward * targetSpeed;

           
            rigidbody.AddRelativeForce(linearForce * rigidbody.drag * Time.fixedDeltaTime, ForceMode.VelocityChange);


            float rudderForce = Mathf.Max(localVelocity.z, propulsionComponent.throttle * maxSpeed) / maxSpeed;

            float targetAngular = rudderForce * propulsionComponent.rudder * propulsionComponent.maxAngleTurnSpeed * Mathf.Deg2Rad;
            float angleDelta = targetAngular - rigidbody.angularVelocity.y;
            angleDelta *= propulsionComponent.angleTurnAcceleration;
            targetAngular += angleDelta;


            rigidbody.AddTorque(targetAngular * rigidbody.transform.up * rigidbody.angularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

            float normalizeSpeed = Mathf.Clamp01(Mathf.Clamp(localVelocity.z, 0, maxSpeed) / maxSpeed);

            float heeling = rigidbody.angularVelocity.y / (propulsionComponent.maxAngleTurnSpeed * Mathf.Deg2Rad) * normalizeSpeed;

            float angleHeeling = heeling * propulsionComponent.maxAngleHeeling * Mathf.Deg2Rad;

            rigidbody.AddRelativeTorque(new Vector3(0,0, angleHeeling * rigidbody.angularDrag * Time.fixedDeltaTime), ForceMode.VelocityChange);

            propulsionComponent.speed = SpeedCovertor.Convert(localVelocity.z, SpeedUnit.M_PER_S, propulsionComponent.unit);
        }
    }
}
