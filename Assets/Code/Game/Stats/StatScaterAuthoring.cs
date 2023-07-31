using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScaterAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Factor = 1;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var stat = ref ecsWorld.GetPool<StatScaterComponent>().AddOrGet(entity);
        stat.factor = m_Factor;
    }
}
