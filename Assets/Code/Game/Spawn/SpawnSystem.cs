using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using System;

public class SpawnSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<Spawn> m_PoolSpawner;
    private EcsPool<Team> m_PoolTeam;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<Spawn>().End();
        m_PoolSpawner = m_World.GetPool<Spawn>();
        m_PoolTeam = m_World.GetPool<Team>();
    }

    public void Run(IEcsSystems systems)
    {
        var entityDatabase = systems.GetData<EntityDatabase>();
        List<GameObject> spaned = new List<GameObject>();
        foreach (var entity in m_Filter)
        {
            ref var spawn = ref m_PoolSpawner.Get(entity);
            var entityData = entityDatabase.GetEntityByID(spawn.entityId);
            var instance = Instantiate(entityData.prefab, spawn.position, spawn.rotation);
            var newEntity = m_World.Bake(instance);
            ref var team = ref m_PoolTeam.Add(newEntity);
            team.id = spawn.teamId;
            m_World.DelEntity(entity);
        }
    }
}
