using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSpeedAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Speed;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var stat = ref ecsWorld.GetPool<StatSpeedComponent>().AddOrGet(entity);
        stat.speed = m_Speed;
    }
}
