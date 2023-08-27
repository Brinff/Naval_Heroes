using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


public static class EcsExtentions
{
    public static int Bake(this EcsWorld world, GameObject gameObject)
    {
        return Bake(world, gameObject.transform);
    }


    public static T GetData<T>(this IEcsSystems systems)
    {
        var datas = systems.GetShared<IEcsData[]>();
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] is T) return (T)datas[i];
        }
        return default(T);
    }

    public static ref T GetSingletonComponent<T>(this EcsFilter filter) where T : struct
    {
        var entity = GetSingleton(filter);  
        return ref filter.GetWorld().GetPool<T>().Get(entity.Value);
    }

    public static int? GetSingleton(this EcsFilter filter)
    {
        var enumerator = filter.GetEnumerator();
        if (enumerator.MoveNext())
        {
            int entity = enumerator.Current;
            enumerator.Dispose();
            return entity;
        }
        return null;
    }

    public static bool IsAny(this EcsFilter filter)
    {
        return filter.GetEntitiesCount() > 0;
    }

    public static T GetSystem<T>(this IEcsSystems systems) where T : IEcsSystem
    {
       return (T)systems.GetAllSystems().Find(x => x is T);
    }

    public static int Bake(this EcsWorld world, Transform transform)
    {
        var poolParentComponent = world.GetPool<ParentComponent>();
        var poolChilds = world.GetPool<ChildsComponent>();
        var poolRoot = world.GetPool<RootComponent>();
        var poolLink = world.GetPool<Link>();

        List<int> entities = new List<int>();
        Bake(world, transform, poolRoot, poolLink, null, ref entities);
        int root = entities.First();

        if (entities.Count > 1)
        {
            ref var childs = ref poolChilds.Add(root);
            childs.entities = new EcsPackedEntity[entities.Count - 1];
            for (int i = 1; i < entities.Count; i++)
            {
                childs.entities[i - 1] = world.PackEntity(entities[i]);
            }
        }



        return root;
    }

    private static void Bake(EcsWorld world, Transform transform, EcsPool<RootComponent> poolRoot, EcsPool<Link> poolLink, Nullable<EcsPackedEntity> root, ref List<int> entities)
    {
        IEntityAuthoring[] entityAuthorings = transform.GetComponents<IEntityAuthoring>();
        if (entityAuthorings.Length > 0)
        {

            bool isAnyEnable = false;
            for (int i = 0; i < entityAuthorings.Length; i++)
            {
                if (entityAuthorings[i].isEnable) isAnyEnable = true;
            }

            if (isAnyEnable)
            {
                int entity = world.NewEntity();

                ref var link = ref poolLink.Add(entity);
                link.transform = transform;

                if (root != null)
                {
                    ref var rootComponent = ref poolRoot.AddOrGet(entity);
                    rootComponent.entity = root.Value;
                }

                foreach (var item in entityAuthorings)
                {
                    if (item.isEnable) item.Bake(entity, world);
                }

                entities.Add(entity);

                if (root == null)
                {
                    root = world.PackEntity(entity);
                }
            }
        }



        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Bake(world, child, poolRoot, poolLink, root, ref entities);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull(this in EcsPackedEntity packed, EcsWorld world)
    {
        return !packed.Unpack(world, out int _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull(this in EcsPackedEntity packed, EcsWorld world)
    {
        return packed.Unpack(world, out int _);
    }
}
