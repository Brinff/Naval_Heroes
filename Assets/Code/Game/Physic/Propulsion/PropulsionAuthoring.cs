using Game;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;


public struct PropulsionComponent
{
    public Rigidbody rigidbody;
    public SpeedUnit unit;
    public float speed;
    public float maxSpeed;
    public float forwardAcceleration;
    public float backwardAcceleration;
    public float throttle;
    public float rudder;
    public VisualEffect exaustEffect;
    public float maxAngleTurnSpeed;
    public float angleTurnAcceleration;
    public float maxAngleHeeling;
}

public class PropulsionAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    public new Rigidbody rigidbody => m_Rigidbody;

    [SerializeField]
    private VisualEffect m_ExaustEffect;

    public SpeedUnit maxMoveUnit = SpeedUnit.KNOTS_PER_H;
    [SerializeField, FormerlySerializedAs("maxMoveSpeed")]
    private float m_MaxSpeed = 34;
    [SerializeField]
    private float m_ForwardAcceleration = 12;
    [SerializeField]
    private float m_BackwardAcceleration = 12;

    [SerializeField]
    private float m_MaxAngleTurnSpeed = 15;
    [SerializeField]
    private float m_AngleTurnAcceleration = 4;

    [SerializeField]
    private float m_MaxAngleHeeling = 20;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var propulsionComponent = ref ecsWorld.GetPool<PropulsionComponent>().Add(entity);
        propulsionComponent.rigidbody = m_Rigidbody;        
        propulsionComponent.unit = maxMoveUnit;
        propulsionComponent.maxSpeed = m_MaxSpeed;
        propulsionComponent.forwardAcceleration = m_ForwardAcceleration;
        propulsionComponent.backwardAcceleration = m_BackwardAcceleration;
        propulsionComponent.speed = 0;
        propulsionComponent.exaustEffect = m_ExaustEffect;
        propulsionComponent.maxAngleTurnSpeed = m_MaxAngleTurnSpeed;
        propulsionComponent.angleTurnAcceleration = m_AngleTurnAcceleration;
        propulsionComponent.maxAngleHeeling = m_MaxAngleHeeling;
    }
}

