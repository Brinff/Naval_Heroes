using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneySoftProvider : MonoBehaviour, ISharedData, ISharedInitalizeData
{
    [SerializeField]
    private int m_StartMoney = 2000;
    private PlayerPrefsData<int> m_MoneySoft;

    public void InitalizeData()
    {
        m_MoneySoft = new PlayerPrefsData<int>(nameof(m_MoneySoft), m_StartMoney);
    }

    public int amount => m_MoneySoft.Value;

    public void AddMoney(int amount)
    {
        m_MoneySoft.Value += amount;
    }

    public bool HasMoney(int amount)
    {
        return m_MoneySoft.Value >= amount;
    }

    public bool SpendMoney(int amount)
    {
        bool hasMoney = HasMoney(amount);
        m_MoneySoft.Value -= amount;
        return hasMoney;
    }
}
