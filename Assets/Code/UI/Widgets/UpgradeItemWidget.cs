using System;
using System.Collections;
using System.Collections.Generic;
using Code.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UpgradeItemWidget : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image m_IconImage;
    [SerializeField]
    private TextMeshProUGUI m_TitleLabel;
    [SerializeField]
    private TextMeshProUGUI m_LevelLabel;
    [SerializeField]
    private TextMeshProUGUI m_CostLabel;
    [SerializeField]
    private Transform m_CostRoot;
    [SerializeField]
    private GameObject m_UpgradeArrow;
    [SerializeField]
    private CanvasGroup m_Group;

    private UpgradeWidget m_UpgradeWidget;
    private UpgradeData m_UpgradeData;

    public void SetIcon(Sprite sprite)
    {
        m_IconImage.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_UpgradeWidget?.OnClickUpgrade(this, m_UpgradeData);
    }

    public void SetClickHandler(UpgradeWidget upgradeWidget)
    {
        m_UpgradeWidget = upgradeWidget;
    }

    public void SetData(UpgradeData upgradeData)
    {
        m_UpgradeData = upgradeData;
    }

    public void SetCost(int amount, bool immediately)
    {
        if (!immediately)
        {
            m_CostRoot.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack).OnComplete(() => { m_CostLabel.text = amount.KiloFormat(); });
            m_CostRoot.transform.DOScale(1f, 0.2f).SetDelay(0.2f);
        }
        else
        {
            m_CostLabel.text = amount.KiloFormatShort();
        }
    }


    public void SetHasMoney(bool hasMoney)
    {
        m_Group.alpha = hasMoney ? 1 : 0.5f;
        m_CostLabel.color = hasMoney ? Color.white : Color.gray;
    }

    public void PlayNoMoney()
    {
        transform.DOScale(0.9f, 0.1f).SetEase(Ease.OutBack);
        transform.DOScale(1f, 0.1f).SetEase(Ease.Linear).SetDelay(0.1f);

        m_CostRoot.DOShakePosition(0.5f, new Vector3(-20, 0, 0));
    }

    public void SetLevel(int level, bool immediately)
    {

        if (!immediately)
        {
            m_LevelLabel.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack).OnComplete(() => { m_LevelLabel.text = $"Level {level}"; });
            m_LevelLabel.transform.DOScale(1f, 0.2f).SetDelay(0.2f);
        }
        else
        {
            m_LevelLabel.text = $"Level {level}";
        }
    }

    public void PlayUpgrade()
    {

        transform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack);
        transform.DOScale(1f, 0.1f).SetEase(Ease.Linear).SetDelay(0.2f);
        GameObject upgradeArrow = Instantiate(m_UpgradeArrow, m_UpgradeArrow.transform.parent, true);
        upgradeArrow.SetActive(true);
        Destroy(upgradeArrow, 0.3f);
    }

    public void SetTitle(string title)
    {
        m_TitleLabel.text = title;
    }

    public bool ContainsData(UpgradeData upgrade)
    {
        return m_UpgradeData == upgrade;
    }
}
