using System;
using Game.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Code.Game.Slots.Stash;
using Code.Services;
using UnityEngine;

public class StashSlot : MonoBehaviour, ISlot
{
    public int id => HasCodeUtility.GetDeterministicHashCode("StashSlot");


    private List<SlotItem> m_Items = new List<SlotItem>();

    public List<SlotItem> items => m_Items;

    public SlotCollection collection { get; private set; }

    [SerializeField]
    private RectTransform[] m_Sockets;

    [Button]
    public void Fill(EntityData[] entities)
    {
        for (int i = 0; i < Mathf.Min(entities.Length, m_Sockets.Length); i++)
        {
            var socket = m_Sockets[i];
            var slotItem = SlotItem.Create(collection, entities[i], socket.position, Quaternion.identity, socket.localScale.x);
            AddItem(slotItem, socket.position);
        }
    }

    public void UpdatePositions()
    {
        for (int i = 0; i < Mathf.Min(m_Items.Count, m_Sockets.Length); i++)
        {
            var socket = m_Sockets[i];
            var item = m_Items[i];
            item.targetPosition = socket.position;
            item.transform.position = socket.position;
        }
    }
    
    public void Clear()
    {
        foreach (var item in m_Items)
        {
            Destroy(item.info.gameObject);
            Destroy(item.entity);
            Destroy(item.gameObject);
        }
        m_Items.Clear();
    }

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (m_Items.Count < 6)
        {
            m_Items.Add(slotItem);
            slotItem.parentSlot = this;
            slotItem.targetPosition = position;
            slotItem.transform.position = position;
            slotItem.gameObject.SetActive(gameObject.activeInHierarchy);
            return true;
        }
        return false;
    }

    public bool AddItemPossible(SlotItem slotItem, Vector3 position)
    {
        return m_Items.Count < 6;
    }

    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;
    }

    public bool RemoveItem(SlotItem slotItem)
    {
        if (m_Items.Remove(slotItem))
        {
            return ServiceLocator.Get<StashService>().RemoveItem(slotItem.entityData);
        }

        return false;
    }

    public bool RemoveItemPossible(SlotItem slotItem, Vector3 position)
    {
        return m_Items.Contains(slotItem);
    }


}
