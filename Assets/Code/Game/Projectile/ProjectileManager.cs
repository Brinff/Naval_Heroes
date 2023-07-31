using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leopotam.EcsLite;
using Game.Paterns;

public class ProjectileManager : Singleton<ProjectileManager>
{
    private EcsWorld m_World;
    private IEcsSystems m_Systems;

    public EcsWorld world => m_World;
    public IEcsSystems systems => m_Systems;

    void Start()
    {
        m_World = new EcsWorld();
        m_Systems = new EcsSystems(m_World);

        var compontens = GetComponents<IEcsSystem>();
        foreach (var item in compontens)
        {
            m_Systems.Add(item);
        }

#if UNITY_EDITOR
        m_Systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
        m_Systems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem());
#endif

        m_Systems.Init();
    }

    void Update()
    {
        m_Systems?.Run();
    }

    void OnDestroy()
    {
        if (m_Systems != null)
        {
            m_Systems.Destroy();
            m_Systems = null;
        }

        if (m_World != null)
        {
            m_World.Destroy();
            m_World = null;
        }
    }

    public int NewProjectile()
    {
        return world.NewEntity();
    }

    public ref T AddComponent<T>(int entity) where T : struct
    {
        var pool = world.GetPool<T>();
        return ref pool.Add(entity);
    }
}
