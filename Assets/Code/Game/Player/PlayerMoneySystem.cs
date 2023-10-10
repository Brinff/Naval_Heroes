using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneySystem : MonoBehaviour, IEcsInitSystem, IEcsGroup<Update>
{
    [SerializeField]
    private int m_StartMoney = 2000;
    private PlayerPrefsData<int> m_MoneySoft;
    public int amount => m_MoneySoft.Value;

    public delegate void ChangeValue();

    public event ChangeValue OnChangeValue;

    public void AddMoney(int amount)
    {
        m_MoneySoft.Value += amount;
        OnChangeValue?.Invoke();
    }

    public bool HasMoney(int amount)
    {
        return m_MoneySoft.Value >= amount;
    }

    public bool SpendMoney(int amount)
    {
        bool hasMoney = HasMoney(amount);
        if (hasMoney)
        {
            m_MoneySoft.Value -= amount;
            OnChangeValue?.Invoke();
        }
        return hasMoney;
    }

    public void Init(IEcsSystems systems)
    {
        m_MoneySoft = new PlayerPrefsData<int>(nameof(m_MoneySoft), m_StartMoney);
    }
}
