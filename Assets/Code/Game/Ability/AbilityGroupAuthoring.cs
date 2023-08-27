using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class AbilityGroupAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private bool m_IsSeparately;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var abilityGroup = ref ecsWorld.GetPool<AbilityGroup>().Add(entity);
        abilityGroup.isSeparately = m_IsSeparately;
    }
}
