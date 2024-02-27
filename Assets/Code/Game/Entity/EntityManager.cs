using Game.Paterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using System.Linq;
using System;
using Sirenix.Utilities;
using UnityEngine.PlayerLoop;

public class Update : Group<Update>
{

}

public class FixedUpdate : Group<FixedUpdate>
{

}

public class DrawGizmos : Group<FixedUpdate>
{

}

public class Group<T> : IEcsGroup where T : IEcsGroup
{
    private EcsSystems m_EcsSystem;

    public void Add(IEcsSystem system)
    {
        if (system is IEcsGroup<T>) m_EcsSystem.Add(system);
    }

    public void BeginInit(EcsWorld world, IEcsData[] data)
    {
        m_EcsSystem = new EcsSystems(world, data);
    }

    public void Dispose()
    {
        m_EcsSystem.Destroy();
        m_EcsSystem = null;
    }

    public void EndInit(bool isEditor)
    {
        if (isEditor)
        {
#if UNITY_EDITOR
            m_EcsSystem.Add(new EcsWorldViewSystem());
            m_EcsSystem.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
            m_EcsSystem.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif
        }

        m_EcsSystem.Init();
    }

    public void Run()
    {
        m_EcsSystem.Run();
    }
}


public interface IEcsGroup : IDisposable
{
    public void Run();
    public void Add(IEcsSystem system);
    public void BeginInit(EcsWorld world, IEcsData[] data);
    public void EndInit(bool isEditor);
}

public interface IEcsGroup<T> where T : IEcsGroup
{

}

public interface IEcsGroupUpdateSystem : IEcsSystem
{

}

public interface IEcsGroupGizmosSystem : IEcsSystem
{

}

public interface IEcsGroupFixedUpdateSystem : IEcsSystem
{

}

public struct TransformComponent
{
    public Transform transform;
}

public struct Parent
{
    public int entity;
}



public struct Childs
{
    public EcsPackedEntity[] entities;
}

public struct Link
{
    public Transform transform;
}

public class EntityManager : Singleton<EntityManager>, ISingletonSetup
{
    [SerializeField]
    private bool m_IsEditor;
    private EcsWorld m_World;
    public EcsWorld world => m_World;
    private List<IEcsGroup> m_Groups = new List<IEcsGroup>();

    private IEcsData[] m_Data;

    public void Run<T>() where T : IEcsGroup
    {
        for (int i = 0; i < m_Groups.Count; i++)
        {
            if (m_Groups[i] is T) m_Groups[i].Run();
        }
    }

    //    private IEcsSystems m_UpdateSystems;
    //    private IEcsSystems m_FixedUpdateSystems;

    //#if UNITY_EDITOR
    //    private IEcsSystems m_GizmosSystems;
    //#endif



    //public IEcsSystems systems => m_UpdateSystems;

    //[SerializeField]
    //private Transform[] m_BakeEntities;

    private void Update()
    {
        Run<Update>();
        //m_UpdateSystems?.Run();
    }

    private void FixedUpdate()
    {
        Run<FixedUpdate>();
        //m_FixedUpdateSystems?.Run();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Run<DrawGizmos>();
        //if (m_GizmosSystems != null) m_GizmosSystems.Run();
    }

#endif

    void OnDestroy()
    {
        for (int i = 0; i < m_Groups.Count; i++)
        {
            m_Groups[i].Dispose();
        }
        m_Groups.Clear();

       

        if (m_World != null)
        {
            m_World.Destroy();
            m_World = null;
        }

        m_Data = null;
    }

    public void Setup()
    {
        m_World = new EcsWorld();

        var datas = new List<IEcsData>(transform.GetComponentsInChildren<IEcsData>());
        var dataProviders = transform.GetComponentsInChildren<IEcsDataProvider>();

        foreach (var dataProvider in dataProviders)
        {
            datas.AddRange(dataProvider.ProvideData());
        }

        m_Data = datas.ToArray();

        var systems = GetComponentsInChildren<IEcsSystem>();

        m_Groups.Add(new Update());
        m_Groups.Add(new FixedUpdate());
        m_Groups.Add(new DrawGizmos());

        foreach (var group in m_Groups)
        {
            group.BeginInit(m_World, m_Data);

            foreach (var system in systems)
            {
                group.Add(system);
            }
        }

        foreach (var group in m_Groups)
        {
            group.EndInit(m_IsEditor);
        }
    }
}
