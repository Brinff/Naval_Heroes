using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CompassEnemyIndicatorComponent
{
    public CompassEnemyIndicator indicator;
}

public class PlayerCompasSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_PlayerShipFilter;
    private EcsFilter m_PlayerEyeFilter;
    private EcsFilter m_NewEnemyFilter;
    private EcsFilter m_EnemyFilter;
    private EcsFilter m_DeadEnemyFilter;
    private EcsFilter m_DestroyEnemyFilter;
    private CompassWidget m_CompassWidget;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<CompassEnemyIndicatorComponent> m_CompassEnemyIndicator;
    public void Init(IEcsSystems systems)
    {
        m_CompassWidget = UISystem.Instance.GetElement<CompassWidget>();
        m_World = systems.GetWorld();
        m_PlayerShipFilter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Inc<TransformComponent>().End();
        m_PlayerEyeFilter = m_World.Filter<PlayerTag>().Inc<EyeComponent>().End();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolEye = m_World.GetPool<EyeComponent>();
        m_CompassEnemyIndicator = m_World.GetPool<CompassEnemyIndicatorComponent>();

        m_NewEnemyFilter = m_World.Filter<AITag>().Inc<ShipTag>().Inc<NewEntityTag>().Inc<TransformComponent>().Exc<CompassEnemyIndicatorComponent>().End();
        m_EnemyFilter = m_World.Filter<AITag>().Inc<ShipTag>().Inc<TransformComponent>().Inc<CompassEnemyIndicatorComponent>().End();
        m_DeadEnemyFilter = m_World.Filter<AITag>().Inc<ShipTag>().Inc<CompassEnemyIndicatorComponent>().Inc<DeadTag>().End();
        m_DestroyEnemyFilter = m_World.Filter<AITag>().Inc<ShipTag>().Inc<CompassEnemyIndicatorComponent>().Inc<DestroyComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var playerShipEntity = m_PlayerShipFilter.GetSingleton();

        if (playerShipEntity != null)
        {
            ref var transform = ref m_PoolTransform.Get(playerShipEntity.Value);
            m_CompassWidget.SetForward(transform.transform.rotation);
            m_CompassWidget.SetPosition(transform.transform.position);
        }

        var playerEyeEntity = m_PlayerEyeFilter.GetSingleton();

        if (playerEyeEntity != null)
        {
            ref var eye = ref m_PoolEye.Get(playerEyeEntity.Value);
            m_CompassWidget.SetRotation(eye.transform.rotation);
        }

        foreach (var entity in m_NewEnemyFilter)
        {
            ref var transform = ref m_PoolTransform.Get(entity);
            ref var compassEnemyIndicator = ref m_CompassEnemyIndicator.Add(entity);

            compassEnemyIndicator.indicator = m_CompassWidget.CreateEnemyIndicator(transform.transform.position);
            compassEnemyIndicator.indicator.Show();
        }

        foreach (var entity in m_EnemyFilter)
        {
            ref var transform = ref m_PoolTransform.Get(entity);
            ref var compassEnemyIndicator = ref m_CompassEnemyIndicator.Get(entity);

            m_CompassWidget.UpdateIndicator(compassEnemyIndicator.indicator.rectTransform, transform.transform.position);
        }

        foreach (var entity in m_DeadEnemyFilter)
        {
            ref var transform = ref m_PoolTransform.Get(entity);
            ref var compassEnemyIndicator = ref m_CompassEnemyIndicator.Get(entity);
            compassEnemyIndicator.indicator.Hide(false);
            m_CompassEnemyIndicator.Del(entity);
        }

        foreach (var entity in m_DestroyEnemyFilter)
        {
            ref var transform = ref m_PoolTransform.Get(entity);
            ref var compassEnemyIndicator = ref m_CompassEnemyIndicator.Get(entity);
            compassEnemyIndicator.indicator.Hide(true);
            m_CompassEnemyIndicator.Del(entity);
        }
    }
}
