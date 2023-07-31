using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoftMoneyCounterWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private TextMeshProUGUI m_AmountLabel;
    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }

    public void SetMoney(int amount)
    {
        m_AmountLabel.text = amount.KiloFormat();
    }
}
