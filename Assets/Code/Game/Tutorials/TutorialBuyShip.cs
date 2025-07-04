using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Services;
using UnityEngine;
using static PlayerSlotsSystem;


public interface ITutorial
{
    void Prepare(EcsWorld ecsWorld, IEcsSystems systems);
    bool ConditionLaunch(EcsWorld ecsWorld, IEcsSystems systems);
    bool ConditionDone(EcsWorld ecsWorld, IEcsSystems systems);
    bool IsDone();
    void Done(EcsWorld ecsWorld, IEcsSystems systems);
    void Process(EcsWorld ecsWorld, IEcsSystems systems);
    void Launch(EcsWorld ecsWorld, IEcsSystems systems);
}

public class TutorialBuyShip : MonoBehaviour, ITutorial
{
    
    private SlotCollection m_SlotCollection;
    private CommandSystem m_CommandSystem;
    private PlayerPrefsData<bool> m_IsDone;
    private TutorialTapWidget m_TutorialTapWidget;
    private SlotItem m_SlotItem;

    public void Prepare(EcsWorld ecsWorld, IEcsSystems systems)
    {
        m_TutorialTapWidget = ServiceLocator.Get<UIRoot>().GetWidget<TutorialTapWidget>();
        m_SlotCollection = systems.GetSystem<PlayerSlotsSystem>().slotCollection;
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_IsDone = new PlayerPrefsData<bool>($"{nameof(TutorialBuyShip)}_{nameof(m_IsDone)}", false);
    }

    public bool ConditionLaunch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotMerge>().All(x => !x.item);
    }

    public bool ConditionDone(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return m_SlotCollection.GetSlots<SlotMerge>().Any(x => x.item);
    }

    public void Done(EcsWorld ecsWorld, IEcsSystems systems)
    {
        var slot = m_SlotCollection.GetSlots<SlotBuy>().FirstOrDefault();
        
        TargetRaycastMediator.Instance.RemoveTargetRaycast(slot.gameObject);
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = false;
        m_TutorialTapWidget.Hide(false);

        var messageWidget = ServiceLocator.Get<UIRoot>().GetWidget<MessageWidget>();
        messageWidget.Hide(false);
        if (m_SlotItem != null) m_SlotItem.isLockDrag = false;
        m_IsDone.Value = true;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
        StartCoroutine(DelayLaunch());
    }

    private IEnumerator DelayLaunch()
    {
        var slot = m_SlotCollection.GetSlots<SlotBuy>().FirstOrDefault();
        m_SlotItem = slot.item;
        m_SlotItem.isLockDrag = true;

        yield return new WaitForSeconds(0.4f);

        var messageWidget = ServiceLocator.Get<UIRoot>().GetWidget<MessageWidget>();
        messageWidget.SetText("TAP THE BUTTON TO BUY A SHIP");
        messageWidget.Show(false);

        //var slot = m_SlotCollection.GetSlots<SlotBuy>().FirstOrDefault();
        Transform transform = slot.GetComponentInChildren<TutorialPoint>().transform;
        m_TutorialTapWidget.PlaceAtWorld(transform.position);
        TargetRaycastMediator.Instance.AddTargetRaycast(slot.gameObject);
        
        m_TutorialTapWidget.Show(false);
    }

    public void Process(EcsWorld ecsWorld, IEcsSystems systems)
    {
        //var slot = m_SlotCollection.GetSlots<SlotBuy>().FirstOrDefault();
        //m_TutorialTapWidget.PlaceAtWorld(slot.transform.position);
    }

    public bool IsDone()
    {
        return m_IsDone.Value;
    }
}
