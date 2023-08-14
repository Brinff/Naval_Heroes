using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerCommand : MonoBehaviour, ICommand
{
    [SerializeField]
    private EntityData entityData;
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        
        //var instance = Instantiate(entityData.prefab);
        //var entity = world.Bake(instance);
        //world.GetPool<PlayerTag>().Add(entity);
    }
}
