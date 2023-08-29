using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDamageAuthoring : StatAuthoring, IEntityAuthoring
{

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var stat = ref ecsWorld.GetPool<StatDamageComponent>().AddOrGet(entity);
        stat.oprtion = m_Option;
        stat.value = m_Value;
    }
}
