using Game;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;
using Leopotam.EcsLite;

public class InputPlayerMovementSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    [Range(-0.5f, 1)]
    [SerializeField]
    private float m_Throttle = 0;
    [Range(-1f, 1f)]
    [SerializeField]
    private float m_Rudder = 0;

    private MovementInputButtonsWidget m_InputWidget;

    private void OnEnable()
    {

    }

    public float GetRudder()
    {
        return m_Rudder;
    }

    public float GetThrotle()
    {
        return m_Throttle;
    }

    private void OnChangeAxisValue(Vector2 axis)
    {
        m_Throttle = axis.y;
        m_Rudder = axis.x;

        m_Throttle = Mathf.Clamp(m_Throttle, -0.5f, 1f);
        m_Rudder = Mathf.Clamp(m_Rudder, -1f, 1f);
    }

    public void SetSpeed(float speed, SpeedUnit units)
    {
        m_InputWidget.SetSpeed(speed, units);
    }


    private EcsFilter m_Filter;
    private EcsPool<PropulsionComponent> m_PoolPropulsionComponent;

    public void Init(IEcsSystems systems)
    {
        m_InputWidget = ServiceLocator.Get<UIController>().GetElement<MovementInputButtonsWidget>();
        m_InputWidget.OnChangeAxisValue += OnChangeAxisValue;

        var world = systems.GetWorld();
        m_Filter = world.Filter<PlayerTag>().Inc<PropulsionComponent>().End();
        m_PoolPropulsionComponent = world.GetPool<PropulsionComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var propulsionComponent = ref m_PoolPropulsionComponent.Get(entity);
            SetSpeed(propulsionComponent.speed, propulsionComponent.unit);
            propulsionComponent.rudder = GetRudder();
            propulsionComponent.throttle = GetThrotle();
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        if (m_InputWidget != null)
        {
            m_InputWidget.OnChangeAxisValue -= OnChangeAxisValue;
        }
    }
}
