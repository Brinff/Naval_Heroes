using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatFireRandomDelayAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private StatOperation m_Option = StatOperation.Overwrite;
    [SerializeField]
    private float m_Value = 1;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var statFireRandomDelay = ref ecsWorld.GetPool<StatFireRandomDelayComponent>().AddOrGet(entity);
        statFireRandomDelay.option = m_Option;
        statFireRandomDelay.value = m_Value;
    }
}
