using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDamageAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Factor;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var stat = ref ecsWorld.GetPool<StatDamageComponent>().AddOrGet(entity);
        stat.factor = m_Factor;
    }
}
