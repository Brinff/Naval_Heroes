using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPlayerCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var filterPlayer = world.Filter<PlayerTag>().Inc<ShipTag>().Exc<DeadTag>().End();
        foreach (var entity in filterPlayer)
        {
            world.GetPool<HomeViewActiveEvent>().Add(entity);
            ref var health = ref world.GetPool<HealthComponent>().Get(entity);
            health.currentValue = health.maxValue;

            ref var lookAtView = ref world.GetPool<LookAtViewComponent>().Get(entity);
            lookAtView.rotation = Quaternion.identity;
        }
    }
}
