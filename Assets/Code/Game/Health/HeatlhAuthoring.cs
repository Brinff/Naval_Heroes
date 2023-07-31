using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealthComponent
{
    public float currentValue;
    public float maxValue;
}

public class HeatlhAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float health;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var heatlhComponent = ref ecsWorld.GetPool<HealthComponent>().AddOrGet(entity);
        heatlhComponent.currentValue = heatlhComponent.maxValue = health;
    }
}
