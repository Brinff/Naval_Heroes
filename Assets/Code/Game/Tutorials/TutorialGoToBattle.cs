using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Game.States;
using Code.Services;
using UnityEngine;



public class TutorialGoToBattle : MonoBehaviour, ITutorial
{

    private SlotCollection m_SlotCollection;
    private CommandSystem m_CommandSystem;
    private PlayerPrefsData<bool> m_IsDone;
    private TutorialTapWidget m_TutorialTapWidget;

    public void Prepare(EcsWorld ecsWorld, IEcsSystems systems)
    {
        m_TutorialTapWidget = ServiceLocator.Get<UIController>().GetWidget<TutorialTapWidget>();
        m_SlotCollection = systems.GetSystem<PlayerSlotsSystem>().slotCollection;
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_IsDone = new PlayerPrefsData<bool>($"{nameof(TutorialGoToBattle)}_{nameof(m_IsDone)}", false);
    }

    public bool ConditionLaunch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotBattleGrid>().Any(x => x.items.Count > 0);
    }

    public bool ConditionDone(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && ServiceLocator.Get<GameStateMachine>().currentState is BattleState;
    }

    public void Done(EcsWorld ecsWorld, IEcsSystems systems)
    {
        var messageWidget = ServiceLocator.Get<UIController>().GetWidget<MessageWidget>();
        messageWidget.Hide(false);

        var button = ServiceLocator.Get<UIController>().GetWidget<StartGameWidget>().GetButton();
        TargetRaycastMediator.Instance.RemoveTargetRaycast(button.gameObject);
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = false;
        m_TutorialTapWidget.Hide(false);
        m_IsDone.Value = true;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
        StartCoroutine(DoLaunch());
    }

    private IEnumerator DoLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        var messageWidget = ServiceLocator.Get<UIController>().GetWidget<MessageWidget>();
        messageWidget.SetText("TAP THE BUTTON TO START BATTLE");
        messageWidget.Show(false);

        var button = ServiceLocator.Get<UIController>().GetWidget<StartGameWidget>().GetButton();
        Transform transform = button.GetComponentInChildren<TutorialPoint>().transform;

        TargetRaycastMediator.Instance.AddTargetRaycast(button.gameObject);

        m_TutorialTapWidget.PlaceAtScreen(transform.position);
        m_TutorialTapWidget.Show(false);
    }

    public void Process(EcsWorld ecsWorld, IEcsSystems systems)
    {

    }

    public bool IsDone()
    {
        return m_IsDone.Value;
    }
}
