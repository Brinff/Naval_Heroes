using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatProjectileVelocityAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private StatOperation m_Option = StatOperation.Overwrite;
    [SerializeField]
    private float m_Value = 1;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var statProjectileVelocity = ref ecsWorld.GetPool<StatProjectileVelocityComponent>().AddOrGet(entity);
        statProjectileVelocity.option = m_Option;
        statProjectileVelocity.value = m_Value;
    }
}
