using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class AbilityStateAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<AbilityState>().Add(entity);
    }
}
