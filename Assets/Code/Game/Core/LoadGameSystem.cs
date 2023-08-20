using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameSystem : MonoBehaviour, IEcsInitSystem, IEcsGroup<Update>
{
    //[SerializeField]
    //private EntityData m_EntityData;

    private EcsWorld m_World;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        var commandSystem = systems.GetSystem<CommandSystem>();
        commandSystem.Execute<CreatePlayerCommand>();
        commandSystem.Execute<GoHomeCommand>();
    }
}
