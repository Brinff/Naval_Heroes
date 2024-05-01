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
using Code.Game.UI.Components;

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
    private BuyAdsShipService m_BuyAdsShipService;


    public int id => name.GetDeterministicHashCode();

    private BoxCollider[] colliders;

    [SerializeField]
    private GameObject m_CostRewardRoot;
    [SerializeField]
    private GameObject m_CostSoftRoot;
    [SerializeField]
    private GameObject m_StashRoot;
    [SerializeField]
    private TextMeshPro m_StashLabel;

    public enum State
    {
        CostSoft, CostReward, Stash 
    }

    private State m_CurrentState;

    public void SetState(State state)
    {
        m_CurrentState = state;
        m_CostRewardRoot.SetActive(state == State.CostReward);
        m_CostSoftRoot.SetActive(state == State.CostSoft);
        m_StashRoot.SetActive(state == State.Stash);
    }


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
        if (m_BuyAdsShipService.isEmpty)
        {
            if (m_BuyCurrencyShipService.IsEnoughCurrency())
            {
                m_BuyCurrencyShipService.Buy();
                items.Remove(slotItem);
                Spawn();
                return true;
            }
        }
        else
        {
            if (m_BuyAdsShipService.Spend())
            {
                items.Remove(slotItem);
                Spawn();
                return true;
            }
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



    public void UpdateState()
    {
        bool isEnoughMoney = m_BuyCurrencyShipService.IsEnoughCurrency();
        bool isReadAds = m_BuyAdsShipService.IsReady();
        if (m_BuyAdsShipService.isEmpty)
        {
            if (isReadAds && !isEnoughMoney)
            {
                SetState(State.CostReward);
            }
            else
            {
                SetState(State.CostSoft);
            }
        }
        else
        {
            SetState(State.Stash);
        }

        m_StashLabel.text = $"In Stock: {m_BuyAdsShipService.countInStash.ToString()}";


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
        //StartCoroutine(DelayCheckMoney());
        UpdateState();
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
        m_BuyAdsShipService = ServiceLocator.Get<BuyAdsShipService>(x => x.category == m_Category);

        m_BuyCurrencyShipService.OnUpdate += UpdateState;
        m_BuyAdsShipService.OnUpdate += UpdateState;
        m_BuyAdsShipService.OnDone += AdsDone;
        UpdateState();

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

        if (m_CurrentState == State.CostReward)
        {
            m_BuyAdsShipService.Show();
        }
        else
        {
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
            UpdateState();
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

    private void AdsDone(bool isDone)
    {
        if (isDone)
        {
            int count = m_BuyAdsShipService.countInStash;
            for (int i = 0; i < count; i++)
            {
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
        }
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
            if (m_BuyAdsShipService.isEmpty)
            {
                return m_BuyCurrencyShipService.IsEnoughCurrency();
            }
            else return true;
            //int money = (int)m_CostShip.GetResult(m_PlayerAmountBuyShip.Value);
           
        }
        return false;
    }
}
