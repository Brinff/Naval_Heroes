using DG.Tweening;
using Game.Grid.Auhoring;
using Game.Utility;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Code.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Code.Game.Slots;
using Code.Services;

using IPointerDragHandler = UnityEngine.EventSystems.IDragHandler;
using IPointerBeginDragHandler = UnityEngine.EventSystems.IBeginDragHandler;
using IPointerEndDragHandler = UnityEngine.EventSystems.IEndDragHandler;
using Code.Game.Slots.Buy;

public class SlotBuy : MonoBehaviour, ISlot, IPointerBeginDragHandler, IPointerEndDragHandler, IPointerDragHandler, IPointerClickHandler, ISlotRenderer
{
    [SerializeField, FormerlySerializedAs("entity")]
    private EntityData m_Entity;
    [SerializeField]
    private ProgressionData m_CostShip;
    [SerializeField]
    private Category m_Category;
    public Category category => m_Category;

    [SerializeField]
    private string m_Key;
    [SerializeField]
    private int m_UnlockReachLevel = -1;
    public List<SlotItem> items { get; private set; } = new List<SlotItem>();
    public SlotItem item => items.Count > 0 ? items[0] : null;
    public SlotCollection collection { get; private set; }

    [SerializeField]
    private TextMeshPro m_CostLabel;
    [SerializeField]
    private SpriteRenderer[] spriteRenderer;
    [SerializeField]
    private Color m_LockColor;
    [SerializeField]
    private Transform m_Socket;

    
    private BuyCurrencyShipService m_BuyCurrencyShipService;



    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;


    public void SetCost(int amount)
    {
        m_CostLabel.text = amount.KiloFormatShort();
    }

    public bool AddItem(SlotItem slotItem, Vector3 position)
    {
        if (items.Count == 0)
        {
            items.Add(slotItem);
            slotItem.parentSlot = this;
            slotItem.targetPosition = position;
            slotItem.transform.position = position;
            slotItem.gameObject.SetActive(gameObject.activeInHierarchy);

            return true;
        }
        return false;
    }


    public bool RemoveItem(SlotItem slotItem)
    {
        if(m_BuyCurrencyShipService.IsEnoughCurrency())
        {
            m_BuyCurrencyShipService.Buy();
            items.Remove(slotItem);
            Spawn();
            return true;
        }
/*        int money = (int)m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
        if (enoughMoney.Invoke(money))
        {
            if (spendMoney.Invoke(money) && items.Remove(slotItem))
            {
                m_PlayerAmountBuyShip.Value++;
                Spawn();
                return true;
            }
        }*/
        return false;
    }



    public void CheckMoney()
    {
        bool isEnoughMoney = m_BuyCurrencyShipService.IsEnoughCurrency();
        foreach (var sprite in spriteRenderer)
        {
            sprite.color = isEnoughMoney ? Color.white : m_LockColor;
        }
        m_CostLabel.color = isEnoughMoney ? Color.white : m_LockColor;
    }

    [Button]
    public void Spawn()
    {

        SetCost(m_BuyCurrencyShipService.cost);
        var slotItem = SlotItem.Create(collection, m_Entity, m_Socket.position, m_Socket.rotation, m_Socket.localScale.x);
        AddItem(slotItem, m_Socket.position);
        StartCoroutine(DelayCheckMoney());

    }

    private IEnumerator DelayCheckMoney()
    {
        yield return null;
        CheckMoney();
    }

    private PlayerPrefsData<int> m_PlayerAmountBuyShip;


    public void Prepare(SlotCollection collection)
    {
        this.collection = collection;

        GridAuhoring grid = gameObject.GetComponent<GridAuhoring>();


        m_PlayerAmountBuyShip = new PlayerPrefsData<int>(nameof(m_PlayerAmountBuyShip) + m_Key);
        //var gridRenderer = GetComponent<GridRendererAuthoring>();
        //gridRenderer.BeginFill(grid.scale, grid.center);

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        RectTransform rectTransform = transform as RectTransform;
        boxCollider.size =  new Vector3(rectTransform.sizeDelta.x + 10, rectTransform.sizeDelta.y + 10, 20);

        colliders = new BoxCollider[] { boxCollider };


        m_BuyCurrencyShipService = ServiceLocator.Get<BuyCurrencyShipService>(x => x.category == m_Category);
        m_BuyCurrencyShipService.OnUpdate += CheckMoney;
        CheckMoney();

        //for (int i = 0; i < grid.rects.Length; i++)
        //{
        //    var sigleRect = grid.GetSingleRect(grid.rects[i]);
        //    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        //    boxCollider.center = new Vector3(sigleRect.center.x, 0, sigleRect.center.y);
        //    boxCollider.size = new Vector3(sigleRect.size.x, 5, sigleRect.size.y);
        //    colliders[i] = boxCollider;
        //    //gridRenderer.AddRect(grid.rects[i].position, grid.rects[i].size);
        //}

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

    private int m_Mission;
    public void SetMissionAmount(int level)
    {
        m_Mission = level;
    }

    public void Show(float duration)
    {
        if (m_Mission >= m_UnlockReachLevel)
        {

            gameObject.SetActive(true);
            CheckMoney();
            item.Show();
            m_CostLabel.DOFade(1, duration);
            foreach (var item in spriteRenderer)
            {
                item.DOFade(1, duration);
            }
        }
        else Hide(0);
        //gridRenderer.DoAlpha(1, duration);
    }

    public void UpdatePositions()
    {
        foreach (var item in items)
        {
            item.position = m_Socket.position;
            item.rotation = m_Socket.rotation;
            item.scale = m_Socket.localScale.x;
            item.targetPosition = m_Socket.position;
            item.targetScale = m_Socket.localScale.x;
            item.targetRotation = m_Socket.rotation;
        }
    }

    public void Hide(float duration)
    {
        gameObject.SetActive(false);
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
        if (slotItem.entityData != null)
        {
            //int money = (int)m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
            return m_BuyCurrencyShipService.IsEnoughCurrency();
        }
        return false;
    }
}
