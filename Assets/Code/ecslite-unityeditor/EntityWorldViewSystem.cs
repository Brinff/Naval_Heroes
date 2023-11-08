using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;

public struct EcsViewEntity
{
    public int entity;
    public int gen;
    public string name;
    public int parent;

    public override int GetHashCode()
    {
        return HashCode.Combine(entity, gen);
    }
}
#if UNITY_EDITOR
public class EcsWorldViewSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem, IEcsPostDestroySystem, IEcsWorldEventListener
{
    private string m_Name = "defaultWorld";
    private EcsWorld m_World;
    private List<EcsViewEntity> m_Entities = new List<EcsViewEntity>();
    private static List<EcsWorldViewSystem> s_Views;

    private EcsPool<Root> m_PoolRoot;
    private EcsPool<Name> m_PoolName;

    public string name => m_Name;
    public EcsWorld world => m_World;
    public List<EcsViewEntity> entities => m_Entities;
    public static List<EcsWorldViewSystem> views => s_Views;

    public delegate void ViewSystemDelegate(EcsWorldViewSystem worldViewSystem);
    public delegate void ViewEntityDelegate(EcsViewEntity view);

    public static event ViewSystemDelegate OnCreateViewSystem;

    public static event ViewSystemDelegate OnDestroyViewSystem;
    public event ViewEntityDelegate OnCreateViewEntity;
    public event ViewEntityDelegate OnChangeViewEntity;
    public event ViewEntityDelegate OnDestroyViewEntity;


    public void PreInit(IEcsSystems systems)
    {
        if (s_Views == null) s_Views = new List<EcsWorldViewSystem>();
        s_Views.Add(this);

        m_World = systems.GetWorld();
        m_PoolName = m_World.GetPool<Name>();
        m_PoolRoot = m_World.GetPool<Root>();

        m_World.AddEventListener(this);


        var entities = Array.Empty<int>();
        var entitiesCount = m_World.GetAllEntities(ref entities);
        for (var i = 0; i < entitiesCount; i++)
        {
            OnEntityCreated(entities[i]);
        }


        OnCreateViewSystem?.Invoke(this);
    }

    public void Init(IEcsSystems systems)
    {


    }

    public void PostDestroy(IEcsSystems systems)
    {
        if (s_Views.Contains(this)) s_Views.Remove(this);
        if (s_Views.Count == 0) s_Views = null;
        OnDestroyViewSystem?.Invoke(this);
    }

    public void Run(IEcsSystems systems)
    {
        //var entities = Array.Empty<int>();
        //var entitiesCount = m_World.GetAllEntities(ref entities);
        for (var i = 0; i < m_Entities.Count; i++)
        {
            var entityView = m_Entities[i];
            if (m_World.PackEntity(entityView.entity).Unpack(m_World, out int realEntity))
            {
                var gen = m_World.GetEntityGen(realEntity);
                var name = GetEntityName(realEntity, gen);
                if (name != entityView.name)
                {
                    entityView.name = name;
                    OnChangeViewEntity?.Invoke(entityView);
                }
            }
            m_Entities[i] = entityView;
        }

        //for (int i = 0; i < m_Entities.Count; i++)
        //{
        //    bool isDestroy = true;
        //    for (int j = 0; j < entitiesCount; j++)
        //    {
        //        var entity = entities[j];
        //        var gen = m_World.GetEntityGen(entity);
        //        int parent = 0;
        //        int hash = HashCode.Combine(entity, gen, parent);

        //        if (hash == m_Entities[i].GetHashCode())
        //        {
        //            isDestroy = false;
        //            break;
        //        }
        //    }

        //    if (isDestroy)
        //    {
        //        Debug.Log("Destory: ");
        //        //OnDestroyViewEntity?.Invoke(entityView);
        //        //m_Entities.RemoveAt(i);
        //    }
        //}


        //foreach (var item in m_Entities)
        //{



        //    bool isDestroy = true;
        //    for (int i = 0; i < entitiesCount; i++)
        //    {
        //        var entity = entities[i];
        //        var gen = m_World.GetEntityGen(entity);
        //        int hash = HashCode.Combine(entity, gen);

        //        if (hash == item.Key)
        //        {
        //            isDestroy = false;
        //            break;
        //        }
        //    }

        //    if (!isDestroy)
        //    {
        //        if (m_PoolRoot.Has(item.Value.id))
        //        {
        //            var value = item.Value;

        //            ref var root = ref m_PoolRoot.Get(value.id);
        //            int parent = 0;
        //            if (root.entity.Unpack(m_World, out int rootEntity))
        //            {
        //                parent = HashCode.Combine(rootEntity, m_World.GetEntityGen(rootEntity));
        //            } 

        //            if (value.parent != parent)
        //            {
        //                value.parent = parent;
        //                m_ChangeEntities.Add(value);
        //                Debug.Log(value.parent);
        //            }
        //        }
        //    }

        //    if (isDestroy)
        //    {
        //        m_DestoryEntities.Add(item.Key);              
        //    }
        //}

        //foreach (var item in m_ChangeEntities)
        //{
        //    var hash = item.GetHashCode();
        //    if (m_Entities.ContainsKey(hash))
        //    {
        //        m_Entities[item.GetHashCode()] = item;
        //        Debug.Log($"Change parent {hash} {item.parent}");
        //    }
        //    //OnChangeViewEntity?.Invoke(item);
        //}

        //m_ChangeEntities.Clear();

        //foreach (var item in m_DestoryEntities)
        //{
        //    OnDestroyViewEntity?.Invoke(m_Entities[item.GetHashCode()]);
        //    m_Entities.Remove(item);
        //}

        //m_DestoryEntities.Clear();
    }

    //public void OnEntityChanged(int entity)
    //{


    //}

    //public void OnEntityCreated(int entity)
    //{
    //    if (m_Entities == null) m_Entities = new List<EntityWorldView>();



    //    var viewEntity = new EntityWorldView() { id = entity, gen = m_World.GetEntityGen(entity), name = };
    //    m_Entities.Add(viewEntity);
    //    OnCreateViewEntity?.Invoke(viewEntity);
    //}

    //public void OnEntityDestroyed(int entity)
    //{
    //    var packEntity = m_World.PackEntity(entity);
    //    m_Entities.Remove(packEntity);
    //    OnDestroyViewEntity?.Invoke(packEntity);
    //}

    //public void OnFilterCreated(EcsFilter filter)
    //{

    //}

    //public void OnWorldDestroyed(EcsWorld world)
    //{

    //}

    //public void OnWorldResized(int newSize)
    //{

    //}
    private Type[] m_Types;
    public string GetEntityName(int entity, int gen)
    {
        var entityName = entity.ToString("X8");

        if (m_PoolName.Has(entity))
        {
            ref var name = ref m_PoolName.Get(entity);
            entityName = name.value;
        }
        else
        {
            if (gen > 0)
            {
                /*
                var count = m_World.GetComponentTypes(entity, ref m_Types);
                for (var i = 0; i < count; i++)
                {
                    entityName = $"{entityName}:{EditorExtensions.GetCleanGenericTypeName(m_Types[i])}";
                }*/
            }
        }
        return entityName;
    }

    public void OnEntityCreated(int entity)
    {
        var gen = m_World.GetEntityGen(entity);
        EcsViewEntity ecsViewEntity = new EcsViewEntity() { entity = entity, gen = gen, name = GetEntityName(entity, gen) };
        m_Entities.Add(ecsViewEntity);
        OnCreateViewEntity?.Invoke(ecsViewEntity);
    }

    public void OnEntityChanged(int entity)
    {
        //throw new NotImplementedException();
    }

    public void OnEntityDestroyed(int entity)
    {
        var gen = m_World.GetEntityGen(entity);
        EcsViewEntity ecsViewEntity = new EcsViewEntity() { entity = entity, gen = gen, name = "" };
        m_Entities.Remove(ecsViewEntity);
        OnDestroyViewEntity?.Invoke(ecsViewEntity);
    }

    public void OnFilterCreated(EcsFilter filter)
    {
        //throw new NotImplementedException();
    }

    public void OnWorldResized(int newSize)
    {
        //throw new NotImplementedException();
    }

    public void OnWorldDestroyed(EcsWorld world)
    {
        //throw new NotImplementedException();
    }
}
#endif