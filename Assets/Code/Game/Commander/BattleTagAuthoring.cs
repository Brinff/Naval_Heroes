using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTagAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<BattleTag>().Add(entity);
    }
}
