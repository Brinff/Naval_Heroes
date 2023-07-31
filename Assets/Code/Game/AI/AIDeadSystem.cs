using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeadSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsPool<AITag> m_PoolAI;
    private EcsWorld m_World;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<HealthEndEvent>().Inc<AITag>().End();
        m_PoolAI = m_World.GetPool<AITag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            m_PoolAI.Del(entity);
        }
    }
}
