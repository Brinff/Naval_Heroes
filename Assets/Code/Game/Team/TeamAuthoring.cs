using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public struct Team
{
    public int id;
}

public class TeamAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private int m_ID;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var team = ref ecsWorld.GetPool<Team>().Add(entity);
        team.id = m_ID;
    }


    //public int id
    //{
    //    get => m_ID; set => SetTeam(value);
    //}

    //private TeamManager m_TeamManager;

    //private Entity m_Entity;

    //private void SetTeam(int team)
    //{

    //    if (m_ID != team)
    //    {
    //        if (m_TeamManager && m_Entity)
    //        {
    //            m_TeamManager.Unregister(m_Entity);
    //            m_TeamManager.Register(m_ID, m_Entity);
    //        }
    //        m_ID = team;
    //    }   
    //}

    //public void OnRegister(Entity entity)
    //{
    //    if (m_TeamManager == null) m_TeamManager = TeamManager.Instance;
    //    if (m_Entity == null) m_Entity = GetComponent<Entity>();

    //    m_TeamManager.Register(m_ID, m_Entity);
    //}

    //public void OnUnregister(Entity entity)
    //{
    //    m_TeamManager.Unregister(m_Entity);
    //}
}
