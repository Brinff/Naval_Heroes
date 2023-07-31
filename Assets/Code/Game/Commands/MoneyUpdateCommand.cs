using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyUpdateCommand : MonoBehaviour, ICommand
{
    public void Execute(EcsWorld world, IEcsSystems systems)
    {
        SoftMoneyCounterWidget softMoneyCounterWidget = UISystem.Instance.GetElement<SoftMoneyCounterWidget>();
        PlayerMoneySoftProvider playerMoneySoftProvider = systems.GetShared<SharedData>().Get<PlayerMoneySoftProvider>();
        softMoneyCounterWidget.SetMoney(playerMoneySoftProvider.amount);
    }
}
