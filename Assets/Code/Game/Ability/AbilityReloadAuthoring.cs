using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class AbilityReloadAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Duration = 2;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var abilityReload = ref ecsWorld.GetPool<AbilityReload>().Add(entity);
        abilityReload.duration = m_Duration;
    }
}
