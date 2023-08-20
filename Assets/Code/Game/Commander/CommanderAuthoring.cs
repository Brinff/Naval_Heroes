using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<CommanderTag>().Add(entity);
    }
}
