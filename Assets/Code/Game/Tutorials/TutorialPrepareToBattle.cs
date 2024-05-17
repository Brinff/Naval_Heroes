using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Services;
using UnityEngine;

public class TutorialPrepareToBattle : MonoBehaviour, ITutorial
{

    private SlotCollection m_SlotCollection;
    private CommandSystem m_CommandSystem;
    private PlayerPrefsData<bool> m_IsDone;
    private TutorialDragWidget m_TutorialDragWidget;

    private SlotMerge m_SlotA;

    public void Prepare(EcsWorld ecsWorld, IEcsSystems systems)
    {
        m_TutorialDragWidget = ServiceLocator.Get<UIController>().GetWidget<TutorialDragWidget>();
        m_SlotCollection = systems.GetSystem<PlayerSlotsSystem>().slotCollection;
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_IsDone = new PlayerPrefsData<bool>($"{nameof(TutorialPrepareToBattle)}_{nameof(m_IsDone)}", false);
    }

    public bool IsDone()
    {
        return m_IsDone.Value;
    }

    public bool ConditionLaunch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotMerge>().Any(x => x.item);
    }

    public bool ConditionDone(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return m_IsDone.Value || m_SlotCollection.GetSlots<SlotBattleGrid>().Any(x => x.items.Count > 0);
    }

    public void Done(EcsWorld ecsWorld, IEcsSystems systems)
    {
        if (m_SlotA)
        {
            TargetRaycastMediator.Instance.RemoveTargetRaycast(m_SlotA.gameObject);
        }

        var messageWidget = ServiceLocator.Get<UIController>().GetWidget<MessageWidget>();
        messageWidget.Hide(false);

        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = false;
        m_SlotA = null;
        m_TutorialDragWidget.Hide(false);
        m_IsDone.Value = true;
        m_SlotCollection.OnChange -= OnChange;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
        StartCoroutine(DelayLaunch());
    }

    private IEnumerator DelayLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        var messageWidget = ServiceLocator.Get<UIController>().GetWidget<MessageWidget>();
        messageWidget.SetText("DRAG AND DROP TO SET THE SHIP");
        messageWidget.Show(false);

        m_SlotCollection.OnChange += OnChange;
        m_SlotA = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault(x => x.item != null);
        SlotBattleGrid slotB = m_SlotCollection.GetSlots<SlotBattleGrid>().FirstOrDefault();
        if (m_SlotA != null)
        {
            TargetRaycastMediator.Instance.AddTargetRaycast(m_SlotA.gameObject);
            
            m_TutorialDragWidget.PlaceAtWorld(m_SlotA.transform.position, slotB.transform.position);
            m_TutorialDragWidget.Show(false);
        }
        else
        {
            Done(null, null);
        }
    }

    private void OnChange(SlotCollection collection)
    {
        if (!IsDone())
        {

            SlotMerge slotA = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault(x => x.item != null);
            if (m_SlotA != null)
            {
                if (m_SlotA != slotA)
                {
                    TargetRaycastMediator.Instance.RemoveTargetRaycast(m_SlotA.gameObject);
                }
            }
            SlotBattleGrid slotB = m_SlotCollection.GetSlots<SlotBattleGrid>().FirstOrDefault();
            if (slotA != null)
            {
                m_SlotA = slotA;
                TargetRaycastMediator.Instance.AddTargetRaycast(m_SlotA.gameObject);

                m_TutorialDragWidget.PlaceAtWorld(slotA.transform.position, slotB.transform.position);
                m_TutorialDragWidget.Show(false);
            }
            else Done(null, null);
        }
    }

    public void Process(EcsWorld ecsWorld, IEcsSystems systems)
    {

    }
}
