using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAutoUseAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private bool m_IsActive;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var abilityAutoUse = ref ecsWorld.GetPool<AbilityAutoUse>().Add(entity);
        abilityAutoUse.isActive = m_IsActive;
    }
}
