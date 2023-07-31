using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpendCommand : MonoBehaviour, ICommand<int>
{
    public void Execute(EcsWorld world, IEcsSystems systems, int amount)
    {
        SoftMoneyCounterWidget softMoneyCounterWidget = UISystem.Instance.GetElement<SoftMoneyCounterWidget>();
        PlayerMoneySoftProvider playerMoneySoftProvider = systems.GetShared<SharedData>().Get<PlayerMoneySoftProvider>();
        if (playerMoneySoftProvider.SpendMoney(amount)) softMoneyCounterWidget.SetMoney(playerMoneySoftProvider.amount);
    }
}
