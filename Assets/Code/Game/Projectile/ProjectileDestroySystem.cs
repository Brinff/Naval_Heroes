using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct ProjectileDestroyEvent
{

}

public class ProjectileDestroySystem : MonoBehaviour, IEcsInitSystem, IEcsPostRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;

    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<ProjectileDestroyEvent>().End();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var item in m_Filter)
        {
            m_World.DelEntity(item);
        }
    }
}
