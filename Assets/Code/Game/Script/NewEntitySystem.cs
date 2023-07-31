using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEntitySystem : MonoBehaviour, IEcsInitSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<NewEntityTag> m_PoolNewEntityTag;
    private EcsPool<EntityNameComponent> m_PoolEntityName;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<NewEntityTag>().End();
        m_PoolNewEntityTag = m_World.GetPool<NewEntityTag>();
        m_PoolEntityName = m_World.GetPool<EntityNameComponent>();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            if (m_PoolEntityName.Has(entity))
            {
                ref var name = ref m_PoolEntityName.Get(entity);
                Debug.Log($"New Entity: {name.value}");
            }

            m_PoolNewEntityTag.Del(entity);
        }
    }
}
