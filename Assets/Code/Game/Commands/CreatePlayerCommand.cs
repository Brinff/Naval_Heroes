using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerCommand : MonoBehaviour, ICommand
{
    [SerializeField]
    private GameObject player;
    public void Execute(EcsWorld world, IEcsSystems systems)
    {     
        var instance = Instantiate(player);
        var entity = world.Bake(instance);
    }
}
