using DG.Tweening;
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;
using Leopotam.EcsLite;

public class OutputOtherHealthSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    [SerializeField]
    private Camera m_Camera;
    private WorldEnemyWidget m_WorldEnemyWidget;


    private EcsFilter m_Filter;
    private EcsFilter m_FilterEndHealth;
    private EcsFilter m_FilterDestroy;

    private EcsPool<HealthComponent> m_PoolHealthComponent;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<HealthBarComponent> m_PoolHealthBarComponent;
    private EcsPool<Classification> m_PoolClassification;
    private EcsPool<Rare> m_PoolRare;
    private EcsPool<GradeLevel> m_PoolGradeLevel;
    private EcsPool<InfoComponent> m_PoolInfoComponent;
    private EcsPool<DestroyComponent> m_PoolDestroy;

    public void Init(IEcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        m_PoolTeam = ecsWorld.GetPool<Team>();
        m_Filter = ecsWorld.Filter<HealthComponent>().Inc<InfoComponent>().Inc<ShipTag>().Inc<Team>().End();
        m_FilterEndHealth = ecsWorld.Filter<HealthBarComponent>().Inc<InfoComponent>().Inc<HealthEndEvent>().Inc<ShipTag>().End();
        m_FilterDestroy = ecsWorld.Filter<HealthBarComponent>().Inc<InfoComponent>().Inc<DestroyComponent>().Inc<ShipTag>().End();

        m_PoolHealthComponent = ecsWorld.GetPool<HealthComponent>();
        m_PoolHealthBarComponent = ecsWorld.GetPool<HealthBarComponent>();
        m_PoolInfoComponent = ecsWorld.GetPool<InfoComponent>();
        m_PoolDestroy = ecsWorld.GetPool<DestroyComponent>();
        m_PoolClassification = ecsWorld.GetPool<Classification>();
        m_PoolRare = ecsWorld.GetPool<Rare>();
        m_PoolGradeLevel = ecsWorld.GetPool<GradeLevel>();
        m_WorldEnemyWidget = ServiceLocator.Get<UIController>().GetWidget<WorldEnemyWidget>();
        m_WorldEnemyWidget.SetWorldCamera(m_Camera);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_FilterEndHealth)
        {
            ref var info = ref m_PoolInfoComponent.Get(item);
            ref var healthBar = ref m_PoolHealthBarComponent.Get(item);
            Transform infoOrigin = info.orgin;
            GameObject healthBarGameObject = healthBar.bar.gameObject;
            healthBar.bar.DoHide(false).OnComplete(()=> m_WorldEnemyWidget.Unregister(healthBarGameObject));
            
        }

        foreach (var entity in m_Filter)
        {
            var healthComponent = m_PoolHealthComponent.Get(entity);
            if (!m_PoolHealthBarComponent.Has(entity))
            {
                ref var healthBarComponent = ref m_PoolHealthBarComponent.Add(entity);

                ref var infoCompoenent = ref m_PoolInfoComponent.Get(entity);
                ref var team = ref m_PoolTeam.Get(entity);

                healthBarComponent.bar = m_WorldEnemyWidget.Register(infoCompoenent.orgin, team.id == 0).GetComponent<HealthBar>();
                healthBarComponent.bar.SetHealth(healthComponent.currentValue, healthComponent.maxValue);


                if (m_PoolClassification.Has(entity))
                {
                    var classificationDatabase = systems.GetData<ClassificationDatabase>();
                    ref var classification = ref m_PoolClassification.Get(entity);
                    healthBarComponent.bar.SetClassification(classificationDatabase.GetById(classification.id).icon);
                }

                if (m_PoolRare.Has(entity))
                {
                    var rareDatabase = systems.GetData<RareDatabase>();
                    ref var rare = ref m_PoolRare.Get(entity);
                    healthBarComponent.bar.SetRare(rareDatabase.GetById(rare.id).color);
                }

                if (m_PoolGradeLevel.Has(entity))
                {
                    ref var gradeLevel = ref m_PoolGradeLevel.Get(entity);
                    healthBarComponent.bar.SetLevel(gradeLevel.amount);
                }

                healthBarComponent.health = healthComponent.currentValue;
            }
            else
            {
                ref var healthBarComponent = ref m_PoolHealthBarComponent.Get(entity);
                ref var infoCompoenent = ref m_PoolInfoComponent.Get(entity);

                m_WorldEnemyWidget.UpdatePosition(healthBarComponent.bar.rectTransform, infoCompoenent.orgin.position);

                if (healthBarComponent.health != healthComponent.currentValue)
                {
                    healthBarComponent.bar.DoHeath(healthComponent.currentValue, healthComponent.maxValue);
                    healthBarComponent.health = healthComponent.currentValue;
                }
            }
        }

        foreach (var entity in m_FilterDestroy)
        {
            ref var destroy = ref m_PoolDestroy.Get(entity);
            if (destroy.delay <= 0)
            {
                ref var healthBarComponent = ref m_PoolHealthBarComponent.Get(entity);
                m_WorldEnemyWidget.Unregister(healthBarComponent.bar.gameObject);
            }
        }
    }
}
