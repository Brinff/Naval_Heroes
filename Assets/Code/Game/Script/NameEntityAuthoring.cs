using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public struct EntityNameComponent
{
    public string value;
}

public class NameEntityAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var name = ref ecsWorld.GetPool<EntityNameComponent>().Add(entity);
        name.value = gameObject.name;
    }
}
