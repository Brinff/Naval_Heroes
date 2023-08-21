using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class AbilityAmmoAmountAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private int m_Current;
    [SerializeField]
    private int m_Max;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var abilityAmmoAmount = ref ecsWorld.GetPool<AbilityAmmoAmount>().Add(entity);
        abilityAmmoAmount.current = m_Current;
        abilityAmmoAmount.max = m_Max;
    }
}
