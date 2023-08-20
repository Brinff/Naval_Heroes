using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_NewFilter;
    private EcsFilter m_KillFilter;
    private EcsFilter m_PlayerAliveFilter;
    private EcsFilter m_PlayerDeadFilter;

    private List<EcsPackedEntity> m_Enemies = new List<EcsPackedEntity>();

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_NewFilter = m_World.Filter<NewEntityTag>().Inc<ShipTag>().Exc<PlayerTag>().End();
        m_KillFilter = m_World.Filter<DeadTag>().Inc<ShipTag>().Exc<PlayerTag>().End();
        m_PlayerAliveFilter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Exc<DeadTag>().End();
        m_PlayerDeadFilter = m_World.Filter<PlayerTag>().Inc<ShipTag>().Inc<DeadTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (m_PlayerDeadFilter.IsAny() && m_Enemies.Count > 0)
        {
            m_Enemies.Clear();
        }

        foreach (var entity in m_KillFilter)
        {
            var packEntity = m_World.PackEntity(entity);
            if (m_Enemies.Contains(packEntity))
            {
                m_Enemies.Remove(packEntity);
                if (m_Enemies.Count <= 0)
                {
                    Debug.Log("Killed all: end game!");
                    if (m_PlayerAliveFilter.IsAny())
                    {
                        Debug.Log("Try no next stage");
                        systems.GetSystem<CommandSystem>().Execute<GoStageCommand>();
                    }
                }
            }
        }

        foreach (var entity in m_NewFilter)
        {
            Debug.Log($"Add entity: {entity}");
            m_Enemies.Add(m_World.PackEntity(entity));
        }
    }
}
