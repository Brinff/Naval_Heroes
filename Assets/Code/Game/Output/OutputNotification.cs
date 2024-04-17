using Game.UI;
using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class OutputPlayerNotificationSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    //private Entity m_Entity;
    //private Team m_Team;
    private PlayerNotificationWidget m_PlayerNotificationWidget;

    private void OnEnable()
    {
        
    }
    //public void OnDamageEndHealth(Entity entity, float damage)
    //{

    //    m_PlayerNotificationWidget.AddNotify<EnemyKillNotify>();
    //    //if (m_Cahche.TryGetValue(entity, out var cache))
    //    //{
    //    //    cache.Item3.WaitOnEndHealth(() => { cache.Item3.DoHide(false).OnComplete(() => { m_WorldEnemyWidget.Unregister(cache.Item2); }); });
    //    //}
    //}

    //public void OnDamageSpendHealth(Entity entity, float damage, float currentHealth, float maxHealth)
    //{
    //    if (m_Cahche.TryGetValue(entity, out var cache))
    //    {
    //        cache.Item3.DoHeath(currentHealth, maxHealth);
    //    }
    //}

    //public void OnTeamAdd(int team, Entity entity)
    //{
    //    Register(entity);
    //}

    //public void OnTeamRemove(int team, Entity entity)
    //{
    //    Unregister(entity);
    //}

    //private Dictionary<Entity, DamageController> m_Cahche = new Dictionary<Entity, DamageController>();

    //public void SetEntity(Entity entity)
    //{
    //    //if (m_Entity != null && m_Entity != entity)
    //    //{

    //    //}

    //    //m_Entity = entity;
    //    ////m_Team = m_Entity.GetComponent<Team>();

    //    //foreach (var team in m_TeamManager.teams)
    //    //{
    //    //    foreach (var otherEntity in team.Value)
    //    //    {
    //    //        Register(otherEntity);
    //    //    }
    //    //}

    //    //m_TeamManager.handle.Add(this);
    //}


    //public void Unregister(Entity entity)
    //{
    //    if (m_Cahche.Remove(entity, out DamageController damageController))
    //    {
    //        damageController.handle.Remove(this);        
    //    }
    //}

    //public void Register(Entity otherEntity)
    //{
    //    if (otherEntity == m_Entity || m_Cahche.ContainsKey(otherEntity)) return;

    //    var otherDamageController = otherEntity.GetComponent<DamageController>();

    //    otherDamageController.handle.Add(this);

    //    m_Cahche.Add(otherEntity, otherDamageController);
    //}


    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<Team> m_PoolTeam;
    public void Init(IEcsSystems systems)
    {
        m_PlayerNotificationWidget = ServiceLocator.Get<UIService>().GetElement<PlayerNotificationWidget>();

        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<HealthEndEvent>().Inc<Team>().Exc<PlayerTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_Filter)
        {
            m_PlayerNotificationWidget.AddNotify<EnemyKillNotify>();
        }
    }
}
