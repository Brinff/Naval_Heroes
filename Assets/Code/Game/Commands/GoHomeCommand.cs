using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeCommand : MonoBehaviour, ICommand
{
    private StartGameWidget m_StartGameWidget;
    private CommandSystem m_CommandSystem;
    
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        PlayerMissionSystem playerMissionSystem = systems.GetSystem<PlayerMissionSystem>();

        


        m_StartGameWidget = UISystem.Instance.GetElement<StartGameWidget>();
        m_StartGameWidget.SetLevel(playerMissionSystem.level);
        m_StartGameWidget.OnClick += OnClickBattle;

        UISystem.Instance.compositionModule.Show<UIHomeComposition>();
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_CommandSystem.Execute<ClearBattleDataCommand>();

        var viewHome = world.Filter<ViewComponent>().Inc<HomeTag>().End().GetSingleton();
        ref var eye = ref world.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
        eye.view = world.PackEntity(viewHome.Value);

        var slotSystem = systems.GetSystem<PlayerSlotsSystem>();
        slotSystem.Show();

        //var filterPlayer = world.Filter<PlayerTagLeo>().Inc<ShipTag>().Exc<DeadTag>().End();
        //var filterEnemy = world.Filter<AITag>().Inc<ShipTag>().Exc<DeadTag>().End();

        //var poolDestroy = world.GetPool<DestroyComponent>();
        //foreach (var entity in filterEnemy)
        //{
        //    if (!poolDestroy.Has(entity)) poolDestroy.Add(entity);
        //}

        //if (!filterPlayer.IsAny())
        //{
        //    m_CommandSystem.Execute<CreatePlayerCommand>();
        //}

        m_CommandSystem.Execute<SetupPlayerCommand>();
        //m_CommandSystem.Execute<UpgradeFillCommand>();
        //m_CommandSystem.Execute<UpgradeUpdateCommand>();
        m_CommandSystem.Execute<MoneyUpdateCommand>();
    }

    private void OnClickBattle()
    {
        m_StartGameWidget.OnClick -= OnClickBattle;
        m_CommandSystem.Execute<GoBattleCommand>();
    }
}
