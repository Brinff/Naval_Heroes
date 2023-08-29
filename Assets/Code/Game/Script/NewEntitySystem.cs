using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEntitySystem : MonoBehaviour, IEcsInitSystem, IEcsPostRunSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<NewEntityTag> m_PoolNewEntityTag;
    private EcsPool<Link> m_PoolLink;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<NewEntityTag>().End();
        m_PoolNewEntityTag = m_World.GetPool<NewEntityTag>();
        m_PoolLink = m_World.GetPool<Link>();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            if (m_PoolLink.Has(entity))
            {
                ref var name = ref m_PoolLink.Get(entity);
                Debug.Log($"New Entity: {name.transform}");
            }

            m_PoolNewEntityTag.Del(entity);
        }
    }
}
