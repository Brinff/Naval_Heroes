using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class ClearBattleDataCommand : MonoBehaviour, ICommand
{

    public void Execute(EcsWorld world, IEcsSystems systems)
    {


        var battleDataFilter = world.Filter<BattleData>().End();
        if (battleDataFilter.IsAny())
        {
            ref var battleData = ref battleDataFilter.GetSingletonComponent<BattleData>();
            foreach (var item in battleData.enemies)
            {
                Destroy(item);
            }
        }

        var filter = world.Filter<ClearBattleTag>().End();
        foreach (var entity in filter)
        {
            world.DelEntity(entity);
        }

        var uiService = ServiceLocator.Get<UIController>();
        uiService.GetElement<AbilityWidget>().Clear();
        uiService.GetElement<WorldEnemyWidget>().Clear();
    }
}
