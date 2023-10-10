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
    [SerializeField]
    private EntityData m_StartShip;
    [SerializeField]
    private SlotBattleGrid m_StartSlot;

    private PlayerPrefsData<List<Slot>> m_PlayerSlots;

    [SerializeField]
    private SlotCollection m_SlotCollection;
    private EntityDatabase m_EntityDatabase;
    private EcsWorld m_World;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<PlayerTag> m_PoolPlayerTag;
    private EcsPool<ClearBattleTag> m_PoolClearBattleTag;
    private PlayerMoneySystem m_PlayerMoneySystem;
    private CommandSystem m_CommandSystem;
    private EcsPool<NewEntityTag> m_PoolNewTag;
    public SlotCollection slotCollection => m_SlotCollection;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolPlayerTag = m_World.GetPool<PlayerTag>();
        m_PoolNewTag = m_World.GetPool<NewEntityTag>();
        m_EntityDatabase = systems.GetData<EntityDatabase>();

        Slot slot = new Slot() { entityId = m_StartShip.id, position = new Vector3(14.289993f, 2.29999447f, -829.839966f), slotId = m_StartSlot.id };

        var misions = systems.GetSystem<PlayerMissionSystem>();

        bool isFirst = GameSettings.Instance.firstLevelisShooter && misions.level == 1;
        if (isFirst) m_PlayerSlots = new PlayerPrefsData<List<Slot>>(nameof(m_PlayerSlots), new List<Slot>() { slot });
        else m_PlayerSlots = new PlayerPrefsData<List<Slot>>(nameof(m_PlayerSlots), new List<Slot>());

        //m_PlayerAmountBuyShip = new PlayerPrefsData<int>(nameof(m_PlayerAmountBuyShip), 0);
        m_PlayerMoneySystem = systems.GetSystem<PlayerMoneySystem>();
        m_CommandSystem = systems.GetSystem<CommandSystem>();
        m_SlotCollection.Prepare();
        m_PoolClearBattleTag = m_World.GetPool<ClearBattleTag>();
        Load();
        m_SlotCollection.OnChange += Save;

        var slotBuys = m_SlotCollection.GetSlots<SlotBuy>();
        //var value = m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
        foreach (var item in slotBuys)
        {
            //item.SetCost((int)value);
            item.spendMoney = SpendMoney;
            item.enoughMoney = EnoughMoney;
        }
    }

    public bool EnoughMoney(int money)
    {
        //var value = m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
        if (m_PlayerMoneySystem.HasMoney(money))
        {
            return true;
        }
        return false;
    }

    public bool SpendMoney(int money)
    {
        //var value = m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
        if (m_PlayerMoneySystem.HasMoney(money))
        {
            m_CommandSystem.Execute<MoneySpendCommand, int>(money);

            //m_PlayerAmountBuyShip.Value++;

            //var slotBuys = m_SlotCollection.GetSlots<SlotBuy>();
            //var newValue = m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
            //foreach (var item in slotBuys)
            //{
            //    item.SetCost((int)newValue);
            //}

            //m_PlayerAmountBuyShip.Save();
            return true;
        }
        return false;
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
            findSlot.AddItem(SlotItem.Create(m_SlotCollection, findEntity, slotData.position, Quaternion.identity, 1), slotData.position);
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
        if (coroutine != null) StopCoroutine(coroutine);

        m_SlotCollection.gameObject.SetActive(true);
        var slots = m_SlotCollection.GetSlots<ISlotRenderer>();
        foreach (var item in slots)
        {
            item.Show(0.3f);
        }
    }

    private Coroutine coroutine;

    private IEnumerator WaitHide()
    {
        yield return new WaitForSeconds(0.3f);
        m_SlotCollection.gameObject.SetActive(false);
    }

    [Button]
    public void Hide()
    {
        if (coroutine != null) StopCoroutine(coroutine);
        var slots = m_SlotCollection.GetSlots<ISlotRenderer>();
        foreach (var item in slots)
        {
            item.Hide(0.3f);
        }
        coroutine = StartCoroutine(WaitHide());
    }

    public void Destroy(IEcsSystems systems)
    {
        m_SlotCollection.OnChange -= Save;
    }


    public bool IsAnyRadyBattle()
    {
        var slots = m_SlotCollection.GetSlots<SlotBattleGrid>();

        foreach (var slot in slots)
        {
            if (slot.items.Count > 0) return true;
        }

        return false;
    }

    public void Bake(IEcsSystems systems)
    {
        var ecb = systems.GetSystem<BeginEntityCommandSystem>().CreateBuffer();
        var slots = m_SlotCollection.GetSlots<SlotBattleGrid>();
        foreach (var slot in slots)
        {
            foreach (var item in slot.items)
            {
                if (item != null && item.entity != null)
                {
                    var entity = m_World.Bake(item.entity, out List<int> entities);
                    m_PoolTeam.Add(entity);
                    m_PoolPlayerTag.Add(entity);
                    ecb.AddComponent<NewEntityTag>(entity);
                    foreach (var childEntity in entities)
                    {
                        m_PoolClearBattleTag.Add(childEntity);
                    }
                }
                else Debug.Log("Null Item or Entity!");
            }
        }
        
    }
}
