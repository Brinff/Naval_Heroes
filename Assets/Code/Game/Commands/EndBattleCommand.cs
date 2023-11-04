using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Tiny.Sauce.Internal.Ads;

public class EndBattleCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        var filter = world.Filter<ShipTag>().End();
        var pool = world.GetPool<Release>();
        foreach (var entity in filter)
        {
            pool.Add(entity);
        }
        
        TSAdsManager.ShowInterstitial();
    }
}
