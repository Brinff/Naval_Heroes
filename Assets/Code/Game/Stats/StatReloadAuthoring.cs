using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatReloadAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_ReloadDuration;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var stat = ref ecsWorld.GetPool<StatReloadComponent>().AddOrGet(entity);
        stat.reloadDuration = m_ReloadDuration;
    }
}
