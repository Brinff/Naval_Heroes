using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private RareData m_RareData;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var rare = ref ecsWorld.GetPool<Rare>().Add(entity);
        rare.id = m_RareData.id;
    }
}
