using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        m_TutorialDragWidget = UISystem.Instance.GetElement<TutorialDragWidget>();
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
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotBattleGrid>().Any(x => x.items.Count > 0);
    }

    public void Done(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.RemoveTargetRaycast(m_SlotA.gameObject);
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = false;
        m_SlotA = null;
        m_TutorialDragWidget.Hide(false);
        m_IsDone.Value = true;
        m_SlotCollection.OnChange -= OnChange;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        StartCoroutine(DelayLaunch());
    }

    private IEnumerator DelayLaunch()
    {
        yield return new WaitForSeconds(1);
        m_SlotCollection.OnChange += OnChange;
        m_SlotA = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault(x => x.item != null);
        SlotBattleGrid slotB = m_SlotCollection.GetSlots<SlotBattleGrid>().FirstOrDefault();
        if (m_SlotA != null)
        {
            TargetRaycastMediator.Instance.AddTargetRaycast(m_SlotA.gameObject);
            TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
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
                    m_SlotA = slotA;
                    TargetRaycastMediator.Instance.AddTargetRaycast(m_SlotA.gameObject);
                }
            }
            SlotBattleGrid slotB = m_SlotCollection.GetSlots<SlotBattleGrid>().FirstOrDefault();
            if (slotA != null)
            {
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
