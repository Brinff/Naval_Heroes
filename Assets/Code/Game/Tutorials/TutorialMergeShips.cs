using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        m_TutorialDragWidget = UISystem.Instance.GetElement<TutorialDragWidget>();
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

    private void DoMerge()
    {
        SlotItem itemA = null;
        var slots = m_SlotCollection.GetSlots<ISlot>();

        ISlot slotB = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault(x => x.item);

        if (slotB != null)
        {
            SlotItem itemB = slotB.items.FirstOrDefault();

            //check slots battle or merge
            foreach (var slot in slots)
            {
                if (slotB == slot || slot is SlotBuy || slot is SlotTrash || slot is SlotDebug) continue;
                itemA = slot.items.FirstOrDefault();
                if (itemA != null) break;
            }

            if (itemA == null)
            {
                //check slots buy
                foreach (var slot in slots)
                {
                    if (slotB == slot || slot is SlotTrash || slot is SlotDebug) continue;
                    itemA = slot.items.FirstOrDefault();
                    if (itemA != null) break;
                }
            }

            m_TutorialDragWidget.PlaceAtWorld(itemA.transform.position, itemB.transform.position);
            m_TutorialDragWidget.Show(false);
        }
        else
        {
            slotB = m_SlotCollection.GetSlots<SlotMerge>().FirstOrDefault();
            foreach (var slot in slots)
            {
                if (slotB == slot || slot is SlotBuy || slot is SlotTrash || slot is SlotDebug) continue;
                itemA = slot.items.FirstOrDefault();
                if (itemA != null) break;
            }

            if (itemA == null)
            {
                //check slots buy
                foreach (var slot in slots)
                {
                    if (slotB == slot || slot is SlotTrash || slot is SlotDebug) continue;
                    itemA = slot.items.FirstOrDefault();
                    if (itemA != null) break;
                }
            }

            m_TutorialDragWidget.PlaceAtWorld(itemA.transform.position, (slotB as SlotMerge).transform.position);
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
        m_TutorialDragWidget.Hide(false);
        m_IsDone.Value = true;
        m_SlotCollection.OnChange -= OnChange;
    }

    public void Launch(EcsWorld ecsWorld, IEcsSystems systems)
    {
        StartCoroutine(DelayLaunch());
        m_SlotCollection.OnChange += OnChange;
    }

    private IEnumerator DelayLaunch()
    {
        yield return new WaitForSeconds(1);
        DoMerge();
    }

    public void Process(EcsWorld ecsWorld, IEcsSystems systems)
    {

    }
}
