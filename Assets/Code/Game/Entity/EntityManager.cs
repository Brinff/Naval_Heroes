using Game.Paterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using System.Linq;
using System;

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

public struct ParentComponent
{
    public int entity;
}

public struct RootComponent
{
    public EcsPackedEntity entity;
}

public struct ChildsComponent
{
    public EcsPackedEntity[] entities;
}

public class EntityManager : Singleton<EntityManager>, ISingletonSetup
{
    private EcsWorld m_World;
    private IEcsSystems m_UpdateSystems;
    private IEcsSystems m_FixedUpdateSystems;

#if UNITY_EDITOR
    private IEcsSystems m_GizmosSystems;
#endif

    public EcsWorld world => m_World;
    public IEcsSystems systems => m_UpdateSystems;

    [SerializeField]
    private Transform[] m_BakeEntities;

    void Start()
    {

    }

    void Update()
    {
        m_UpdateSystems?.Run();
    }

    void FixedUpdate()
    {
        m_FixedUpdateSystems?.Run();
    }

    void OnDestroy()
    {
        if (m_UpdateSystems != null)
        {
            m_UpdateSystems.Destroy();
            m_UpdateSystems = null;
        }

        if (m_FixedUpdateSystems != null)
        {
            m_FixedUpdateSystems.Destroy();
            m_FixedUpdateSystems = null;
        }
#if UNITY_EDITOR
        if (m_GizmosSystems != null)
        {
            m_GizmosSystems.Destroy();
            m_GizmosSystems = null;
        }
#endif
        if (m_World != null)
        {
            m_World.Destroy();
            m_World = null;
        }
    }

    public void Setup()
    {
        m_World = new EcsWorld();

        SharedData sharedData = new SharedData(transform);

        m_UpdateSystems = new EcsSystems(m_World, sharedData);
        m_FixedUpdateSystems = new EcsSystems(m_World);
#if UNITY_EDITOR
        m_GizmosSystems = new EcsSystems(m_World);
#endif
        var compontens = GetComponentsInChildren<IEcsSystem>();
        foreach (var item in compontens)
        {
            if (item is IEcsGroupUpdateSystem) m_UpdateSystems.Add(item);
            if (item is IEcsGroupFixedUpdateSystem) m_FixedUpdateSystems.Add(item);
#if UNITY_EDITOR
            if (item is IEcsGroupGizmosSystem) m_GizmosSystems.Add(item);
#endif
        }

#if UNITY_EDITOR
        m_UpdateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
        m_UpdateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());

        m_FixedUpdateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
        m_FixedUpdateSystems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());


#endif

        m_UpdateSystems.Init();
        m_FixedUpdateSystems.Init();
#if UNITY_EDITOR
        m_GizmosSystems.Init();
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_GizmosSystems != null) m_GizmosSystems.Run();
    }

#endif
    //public int NewEntity(GameObject gameObject)
    //{

    //}
}
