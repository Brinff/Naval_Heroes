using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Linq;

public class AbilityItem : MonoBehaviour, IDisposable, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image m_AbilityIconImage;
    [SerializeField]
    private Image m_AbilityFillImage;
    [SerializeField]
    private AmmoItem m_AmmoItem;
    [SerializeField]
    private GridLayoutGroup m_AmmoGrid;
    [SerializeField]
    private RectTransform m_Button;

    private List<AmmoItem> m_AmmoItems = new List<AmmoItem>();

    private SortAmmo m_SortAmmo = new SortAmmo();

    private void OnEnable()
    {
        m_AmmoItem.gameObject.SetActive(false);
        //m_Button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        //m_Button.onClick.RemoveListener(OnClick);
    }

    public delegate void ClickDelegate(int id);

    public event ClickDelegate OnPerform;

    private int m_Id;

    //private void OnClick()
    //{
        
    //}

    public void SetId(int id)
    {
        m_Id = id;
    }

    public void SetReload(float amount)
    {
        m_AbilityFillImage.fillAmount = Mathf.Clamp01(1 - amount);
    }

    public void SetAbilityIcon(Sprite sprite)
    {
        m_AbilityIconImage.sprite = sprite;
    }

    public AmmoItem CreateAmmoItem()
    {
        var instance = Instantiate(m_AmmoItem);
        instance.transform.SetParent(m_AmmoGrid.transform);
        instance.gameObject.SetActive(true);
        m_AmmoItems.Add(instance);
        return instance;
    }

    public void UpdateAmmo()
    {
        if (m_SortAmmo == null) m_SortAmmo = new SortAmmo();
        m_AmmoItems.Sort(m_SortAmmo);
        //m_AmmoItems = m_AmmoItems.OrderBy(x => x.reload).ToList();
        foreach (var item in m_AmmoItems)
        {
            item.transform.SetAsLastSibling();
        }
    }

    public class SortAmmo : IComparer<AmmoItem>
    {
        public int Compare(AmmoItem x, AmmoItem y)
        {
            if (x.reload < y.reload)
            {
                return 1;
            }
            if (x.reload > y.reload)
            {
                return -1;
            }
            return 0;
        }
    }

    public void Dispose(AmmoItem ammoItem)
    {
        if (m_AmmoItems.Remove(ammoItem))
        {
            ammoItem.Dispose();
        }
    }

    public void SetAmmoIcon(Sprite sprite)
    {
        m_AmmoItem.SetSprite(sprite);
        m_AmmoGrid.cellSize = sprite.rect.size;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    private bool m_IsPerform;

    private void Update()
    {
        if(m_IsPerform) OnPerform?.Invoke(m_Id);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsPerform = true;
        m_Button.DOScale(0.8f, 0.2f).SetEase(Ease.OutCubic);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsPerform = false;
        m_Button.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
}
