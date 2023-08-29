using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System;
using DG.Tweening;

public class SpawnSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsFilter m_BattleDataFilter;
    private EcsPool<Spawn> m_PoolSpawner;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<NewEntityTag> m_PoolNewEntityTag;
    private BeginEntityCommandSystem m_BeginEntityCommandSystem;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<Spawn>().End();
        m_PoolSpawner = m_World.GetPool<Spawn>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolNewEntityTag = m_World.GetPool<NewEntityTag>();
        m_BeginEntityCommandSystem = systems.GetSystem<BeginEntityCommandSystem>();
        m_BattleDataFilter = m_World.Filter<BattleData>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var entityDatabase = systems.GetData<EntityDatabase>();
        foreach (var entity in m_Filter)
        {
            ref var spawn = ref m_PoolSpawner.Get(entity);
            if (spawn.isSpawned) continue;

            var entityData = entityDatabase.GetById(spawn.entityId);

            ref var battleData = ref m_BattleDataFilter.GetSingletonComponent<BattleData>();

            if (battleData.enemies == null) battleData.enemies = new List<GameObject>();

            var instance = Instantiate(entityData.prefab, spawn.position - Vector3.right * 400, spawn.rotation);

            battleData.enemies.Add(instance);

            instance.transform.DOMove(spawn.position, 5);
            var newEntity = m_World.Bake(instance, out List<int> entities);

            var buffer = m_BeginEntityCommandSystem.CreateBuffer();
            buffer.AddComponent<NewEntityTag>(newEntity);
            buffer.AddComponent(newEntity, new Team() { id = spawn.teamId });

            foreach (var item in entities)
            {
                buffer.AddComponent<ClearBattleTag>(item);
            }
             
            spawn.isSpawned = true;

            //m_World.DelEntity(entity);
        }
    }
}
