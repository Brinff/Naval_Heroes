using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Services;
using UnityEngine;

public class TutorialMergeShips : MonoBehaviour, ITutorial
{

    private SlotCollection m_SlotCollection;
    private CommandSystem m_CommandSystem;
    private PlayerPrefsData<bool> m_IsDone;
    private TutorialDragWidget m_TutorialDragWidget;
    private SlotMerge m_TargetSlot;
    public void Prepare(EcsWorld ecsWorld, IEcsSystems systems)
    {
        m_TutorialDragWidget = ServiceLocator.Get<UIController>().GetElement<TutorialDragWidget>();
        m_SlotCollection = systems.GetSystem<PlayerSlotsSystem>().slotCollection;

        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_IsDone = new PlayerPrefsData<bool>($"{nameof(TutorialMergeShips)}_{nameof(m_IsDone)}", false);
    }

    private void OnChange(SlotCollection collection)
    {
        if (!IsDone())
        {
            m_TutorialDragWidget.Hide(true);

            DoMerge();
        }
    }
    private SlotItem m_ItemA;
    private void DoMerge()
    {
        
        var slots = m_SlotCollection.GetSlots<ISlot>();

        ISlot slotB = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault(x => x.item);

        if (slotB != null)
        {
            SlotItem itemB = slotB.items.FirstOrDefault();

            //check slots battle or merge
            foreach (var slot in slots)
            {
                if (slotB == slot || slot is SlotBuy || slot is SlotTrash || slot is SlotDebug) continue;
                m_ItemA = slot.items.FirstOrDefault();
                if (m_ItemA != null) break;
            }

            if (m_ItemA == null)
            {
                //check slots buy
                foreach (var slot in slots)
                {
                    if (slotB == slot || slot is SlotTrash || slot is SlotDebug) continue;
                    m_ItemA = slot.items.FirstOrDefault();
                    if (m_ItemA != null) break;
                }
            }
            TargetRaycastMediator.Instance.AddTargetRaycast(m_ItemA.gameObject);
           
            m_TutorialDragWidget.PlaceAtWorld(m_ItemA.transform.position, itemB.transform.position);
            m_TutorialDragWidget.Show(false);
        }
        else
        {
            slotB = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault();
            foreach (var slot in slots)
            {
                if (slotB == slot || slot is SlotBuy || slot is SlotTrash || slot is SlotDebug) continue;
                m_ItemA = slot.items.FirstOrDefault();
                if (m_ItemA != null) break;
            }

            if (m_ItemA == null)
            {
                //check slots buy
                foreach (var slot in slots)
                {
                    if (slotB == slot || slot is SlotTrash || slot is SlotDebug) continue;
                    m_ItemA = slot.items.FirstOrDefault();
                    if (m_ItemA != null) break;
                }
            }

            TargetRaycastMediator.Instance.AddTargetRaycast(m_ItemA.gameObject);
            TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
            m_TutorialDragWidget.PlaceAtWorld(m_ItemA.transform.position, (slotB as SlotMerge).transform.position);
            m_TutorialDragWidget.Show(false);
        }
    }

    public bool ConditionLaunch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotMerge>().Any(x => x.item);
    }

    public bool ConditionDone(EcsWorld ecsWorld, IEcsSystems systems)
    {
        return !m_IsDone.Value && m_SlotCollection.GetSlots<SlotMerge>().Any(x => x.IsMerged());
    }
    public bool IsDone()
    {
        return m_IsDone.Value;
    }
    public void Done(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.RemoveTargetRaycast(m_ItemA.gameObject);
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = false;
        m_TutorialDragWidget.Hide(false);
        m_IsDone.Value = true;
        m_SlotCollection.OnChange -= OnChange;
        var messageWidget = ServiceLocator.Get<UIController>().GetElement<MessageWidget>();
        messageWidget.Hide(false);
        m_ItemA = null;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        TargetRaycastMediator.Instance.isOverrideTargetRaycasts = true;
        StartCoroutine(DelayLaunch());
        m_SlotCollection.OnChange += OnChange;
    }

    private IEnumerator DelayLaunch()
    {
        yield return new WaitForSeconds(0.4f);
        var messageWidget = ServiceLocator.Get<UIController>().GetElement<MessageWidget>();
        messageWidget.SetText("DRAG AND DROP TO MERGE SHIPS");
        messageWidget.Show(false);
        DoMerge();
    }

    public void Process(EcsWorld ecsWorld, IEcsSystems systems)
    {

    }
}
