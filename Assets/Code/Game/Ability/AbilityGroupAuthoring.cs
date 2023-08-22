using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class AbilityGroupAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<AbilityGroup>().Add(entity);
    }
}
