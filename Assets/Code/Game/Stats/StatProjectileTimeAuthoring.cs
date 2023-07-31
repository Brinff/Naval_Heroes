using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatProjectileTimeAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private StatOperation m_Option = StatOperation.Overwrite;
    [SerializeField]
    private float m_Value;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var statProjectileTime = ref ecsWorld.GetPool<StatProjectileTimeComponent>().AddOrGet(entity);
        statProjectileTime.option = m_Option;
        statProjectileTime.value = m_Value;
    }
}
