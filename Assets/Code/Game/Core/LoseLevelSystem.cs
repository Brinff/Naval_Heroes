using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTagLeo>().Inc<ShipTag>().Inc<HealthEndEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (m_Filter.IsAny())
        {
            var commandSystem = systems.GetSystem<CommandSystem>();
            commandSystem.Execute<GoLoseCommand>();
        }
    }
}
