using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    private StartGameWidget m_StartGameWidget;

    public void Destroy(IEcsSystems systems)
    {
        if (m_StartGameWidget != null) m_StartGameWidget.OnClick -= OnStartGame;
    }

    private bool m_StartGameTrigger = false;

    private EcsFilter m_Filter;
    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_StartGameWidget.OnClick += OnStartGame;
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Exc<PlayerAimPointComponent>().End();
    }

    private void OnStartGame()
    {
        m_StartGameTrigger = true;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var etntiy in m_Filter)
        {
            if (m_StartGameTrigger)
            {


                m_StartGameTrigger = false;
            }
        }
    }
}
