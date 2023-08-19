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
        PlayerLevelProvider playerLevelProvider = systems.GetShared<SharedData>().Get<PlayerLevelProvider>();

        m_StartGameWidget = UISystem.Instance.GetElement<StartGameWidget>();
        m_StartGameWidget.SetLevel(playerLevelProvider.level);
        m_StartGameWidget.OnClick += OnClickBattle;

        UISystem.Instance.compositionModule.Show<UIHomeComposition>();
        m_CommandSystem = systems.GetSystem<CommandSystem>();

        var filterPlayer = world.Filter<PlayerTagLeo>().Inc<ShipTag>().Exc<DeadTag>().End();
        var filterEnemy = world.Filter<AITag>().Inc<ShipTag>().Exc<DeadTag>().End();

        var poolDestroy = world.GetPool<DestroyComponent>();
        foreach (var entity in filterEnemy)
        {
            if (!poolDestroy.Has(entity)) poolDestroy.Add(entity);
        }

        if (!filterPlayer.IsAny())
        {
            m_CommandSystem.Execute<CreatePlayerCommand>();
        }

        m_CommandSystem.Execute<SetupPlayerCommand>();
        m_CommandSystem.Execute<UpgradeFillCommand>();
        m_CommandSystem.Execute<UpgradeUpdateCommand>();
        m_CommandSystem.Execute<MoneyUpdateCommand>();
    }

    private void OnClickBattle()
    {
        Debug.Log("Ete");
        m_StartGameWidget.OnClick -= OnClickBattle;
        m_CommandSystem.Execute<GoBattleCommand>();
    }
}
