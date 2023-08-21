using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAuthorring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private AbilityData m_AbilityData;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var ability = ref ecsWorld.GetPool<Ability>().Add(entity);
        ability.id = m_AbilityData.id;
    }
}
