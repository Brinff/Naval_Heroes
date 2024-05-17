using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class GoHomeCommand : MonoBehaviour, ICommand
{
    private StartGameWidget m_StartGameWidget;
    private PlayerSlotsSystem m_PlayerSlotsSystem;
    private CommandSystem m_CommandSystem;
    private bool m_IsLockBattle;

    private IEnumerator WaitLockBattle()
    {
        m_IsLockBattle = true;
        yield return new WaitForSeconds(GameSettings.Instance.timeLockScreen);
        m_IsLockBattle = false;
    }

    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        PlayerMissionSystem playerMissionSystem = systems.GetSystem<PlayerMissionSystem>();

        StartCoroutine(WaitLockBattle());

        m_PlayerSlotsSystem = systems.GetSystem<PlayerSlotsSystem>();
        m_PlayerSlotsSystem.slotCollection.OnChange += OnChangeSlotCollection;


        m_StartGameWidget = ServiceLocator.Get<UIController>().GetWidget<StartGameWidget>();
        m_StartGameWidget.SetLevel(playerMissionSystem.level);
        m_StartGameWidget.OnClick += OnClickBattle;
        m_StartGameWidget.SetBlock(!m_PlayerSlotsSystem.IsAnyRadyBattle());



        ServiceLocator.Get<UIController>().compositionModule.Show<UIHomeComposition>();
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_CommandSystem.Execute<ClearBattleDataCommand>();

        var viewHome = world.Filter<ViewComponent>().Inc<HomeTag>().End().GetSingleton();
        ref var eye = ref world.Filter<EyeComponent>().End().GetSingletonComponent<EyeComponent>();
        eye.view = world.PackEntity(viewHome.Value);

        var slotSystem = systems.GetSystem<PlayerSlotsSystem>();

        var slots = slotSystem.slotCollection.GetSlots<SlotBuy>();
        foreach (var slot in slots)
        {
            slot.SetMissionAmount(playerMissionSystem.level);
        }

        slotSystem.Show();
        
        //m_CommandSystem.Execute<SetupPlayerCommand>();

        systems.GetSystem<TutorialSystem>().HomeTutorial();
    }

    private void OnChangeSlotCollection(SlotCollection collection)
    {
        m_StartGameWidget.SetBlock(!m_PlayerSlotsSystem.IsAnyRadyBattle());
    }

    private void OnClickBattle()
    {
        if (!m_PlayerSlotsSystem.IsAnyRadyBattle() || m_IsLockBattle)
        {
            return;
        }
        m_StartGameWidget.OnClick -= OnClickBattle;
        m_PlayerSlotsSystem.slotCollection.OnChange -= OnChangeSlotCollection;
        m_CommandSystem.Execute<GoBattleCommand>();
    }
}
