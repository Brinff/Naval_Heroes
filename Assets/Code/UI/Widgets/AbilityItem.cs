using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class AbilityItem : MonoBehaviour, IDisposable
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
    private Button m_Button;

    private List<AmmoItem> m_AmmoItems = new List<AmmoItem>();

    private SortAmmo m_SortAmmo = new SortAmmo();

    private void OnEnable()
    {
        m_AmmoItem.gameObject.SetActive(false);
        m_Button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        m_Button.onClick.RemoveListener(OnClick);
    }

    public delegate void ClickDelegate();

    public event ClickDelegate OnPerform;


    private void OnClick()
    {
        OnPerform?.Invoke();
    }

    public void SetReload(float amount)
    {
        m_AbilityFillImage.fillAmount = 1 - amount;
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
    }

    public class SortAmmo : IComparer<AmmoItem>
    {
        public int Compare(AmmoItem x, AmmoItem y)
        {
            if (x.reload < y.reload)
            {
                x.transform.SetAsLastSibling();
                return 1;
            }
            if (x.reload > y.reload)
            {
                y.transform.SetAsLastSibling();
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
}
