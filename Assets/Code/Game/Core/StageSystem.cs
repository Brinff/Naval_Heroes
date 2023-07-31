using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<LoadStageEvent> m_PoolLoadStageEvent;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTag>().Inc<LevelComponent>().Inc<StageComponent>().Inc<LoadStageEvent>().End();
        m_PoolLoadStageEvent = m_World.GetPool<LoadStageEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {

            m_PoolLoadStageEvent.Del(entity);
        }
    }


}
