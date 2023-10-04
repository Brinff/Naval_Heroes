using DG.Tweening;
using Game.Grid.Auhoring;
using Game.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBuy : MonoBehaviour, ISlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, ISlotRenderer
{
    public EntityData entity;

    public List<SlotItem> items { get; private set; } = new List<SlotItem>();
    public SlotItem item => items.Count > 0 ? items[0] : null;
    public SlotCollection collection { get; private set; }

    [SerializeField]
    private TextMeshPro m_CostLabel;
    [SerializeField]
    private SpriteRenderer[] spriteRenderer;
    [SerializeField]
    private Color m_LockColor;


    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;


    public delegate bool SpendMoney(EntityData entity);

    public SpendMoney spendMoney;

    public delegate bool EnoughMoney(EntityData entity);

    public EnoughMoney enoughMoney;

    public void SetCost(int amount)
    {
        m_CostLabel.text = amount.KiloFormat();
    }

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (items.Count == 0)
        {
            items.Add(slotItem);
            slotItem.parentSlot = this;
            slotItem.targetPosition = position;
            slotItem.transform.position = position;
            return true;
        }
        return false;
    }




    public bool RemoveItem(SlotItem slotItem)
    {
        if (enoughMoney.Invoke(entity))
        {
            if (spendMoney.Invoke(entity) && items.Remove(slotItem))
            {
                Spawn();
                return true;
            }
        }
        return false;
    }


    private void CheckMoney()
    {
        bool isEnoughMoney = enoughMoney?.Invoke(entity) ?? false;
        foreach (var sprite in spriteRenderer)
        {
            sprite.color = isEnoughMoney ? Color.white : m_LockColor;
        }
        m_CostLabel.color = isEnoughMoney ? Color.white : m_LockColor;
    }

    [Button]
    public void Spawn()
    {
        AddItem(SlotItem.Create(collection, entity, transform.position), transform.position);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return null;
        CheckMoney();
    }

    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;

        GridAuhoring grid = gameObject.GetComponent<GridAuhoring>();

        //var gridRenderer = GetComponent<GridRendererAuthoring>();
        //gridRenderer.BeginFill(grid.scale, grid.center);

        colliders = new BoxCollider[grid.rects.Length];
        for (int i = 0; i < grid.rects.Length; i++)
        {
            var sigleRect = grid.GetSingleRect(grid.rects[i]);
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(sigleRect.center.x, 0, sigleRect.center.y);
            boxCollider.size = new Vector3(sigleRect.size.x, 5, sigleRect.size.y);
            colliders[i] = boxCollider;
            //gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        }

        //gridRenderer.EndFill();

        Spawn();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        item.As<IBeginDragHandler>()?.OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item.As<IEndDragHandler>()?.OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        item.As<IDragHandler>()?.OnDrag(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one * 0.8f, 0.1f);
        transform.DOScale(Vector3.one * 1, 0.1f).SetDelay(0.1f);

        var mergeSlots = collection.GetSlots<SlotMerge>();
        foreach (var slot in mergeSlots)
        {
            if (slot.AddItemPossible(item, slot.transform.position))
            {
                var tempItem = item;
                if (RemoveItem(tempItem))
                {
                    if (slot.AddItem(tempItem, slot.transform.position))
                    {
                        break;
                    }
                }
            }
        }
    }

    public void Show(float duration)
    {
        CheckMoney();
        item.Show();
        m_CostLabel.DOFade(1, duration);
        foreach (var item in spriteRenderer)
        {
            item.DOFade(1, duration);
        }
        //gridRenderer.DoAlpha(1, duration);
    }

    public void Hide(float duration)
    {
        item.Hide();
        m_CostLabel.DOFade(0, duration);
        foreach (var item in spriteRenderer)
        {
            item.DOFade(0, duration);
        }
        //gridRenderer.DoAlpha(0, duration);
    }

    public bool AddItemPossible(SlotItem slotItem, Vector3 position)
    {
        return false;
    }

    public bool RemoveItemPossible(SlotItem slotItem, Vector3 position)
    {
        if (slotItem.entityData != null) return enoughMoney.Invoke(slotItem.entityData);
        return false;
    }
}
