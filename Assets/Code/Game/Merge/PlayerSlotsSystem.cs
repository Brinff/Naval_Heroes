using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Linq;
using System;

public class PlayerSlotsSystem : MonoBehaviour, IEcsInitSystem, IEcsDestroySystem, IEcsRunSystem, IEcsGroup<Update>
{
    [System.Serializable]
    public class Slot
    {
        public int slotId;
        public int entityId;
        public Vector3 position;
    }

    private PlayerPrefsData<List<Slot>> m_PlayerSlots;

    [SerializeField]
    private SlotCollection m_SlotCollection;
    private EntityDatabase m_EntityDatabase;
    private EcsWorld m_World;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<PlayerTag> m_PoolPlayerTag;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolPlayerTag = m_World.GetPool<PlayerTag>();
        m_EntityDatabase = systems.GetData<EntityDatabase>();
        m_PlayerSlots = new PlayerPrefsData<List<Slot>>(nameof(m_PlayerSlots), new List<Slot>());
        m_SlotCollection.Prepare();
        Load();
        m_SlotCollection.OnChange += Save;
    }

    public void Run(IEcsSystems systems)
    {

    }

    [Button]
    public void Load()
    {
        foreach (var slotData in m_PlayerSlots.Value)
        {
            var slots = m_SlotCollection.GetSlots<ISlot>();
            var findSlot = slots.First(x => x.id == slotData.slotId);
            var findEntity = m_EntityDatabase.GetById(slotData.entityId);
            findSlot.AddItem(SlotItem.Create(m_SlotCollection, findEntity, slotData.position), slotData.position);
        }
    }

    [Button]
    public void Save(SlotCollection collection)
    {
        m_PlayerSlots.Value.Clear();
        var slots = collection.GetSlots<ISlot>();
        foreach (var slot in slots)
        {
            if (slot is SlotBuy || slot is SlotTrash || slot is SlotDebug) continue;

            var items = slot.items;
            int slotId = slot.id;
            foreach (var item in items)
            {
                Vector3 position = item.position;
                int enitytId = item.entityData.id;
                m_PlayerSlots.Value.Add(new Slot() { slotId = slotId, entityId = enitytId, position = position });
            }
        }
        m_PlayerSlots.Save();
    }

    [Button]
    public void Show()
    {
        var slots = m_SlotCollection.GetSlots<ISlotRenderer>();
        foreach (var item in slots)
        {
            item.Show(0.3f);
        }
    }
    [Button]
    public void Hide()
    {
        var slots = m_SlotCollection.GetSlots<ISlotRenderer>();
        foreach (var item in slots)
        {
            item.Hide(0.3f);
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        m_SlotCollection.OnChange -= Save;
    }

    public void Bake()
    {
        var slots = m_SlotCollection.GetSlots<SlotBattleGrid>();
        foreach (var slot in slots)
        {
            foreach (var item in slot.items)
            {
                var entity = m_World.Bake(item.entity);
                m_PoolTeam.Add(entity);
                m_PoolPlayerTag.Add(entity);
            }
        }
    }
}
