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
        PlayerMoneySystem layerMoneySystem = systems.GetSystem<PlayerMoneySystem>();
        softMoneyCounterWidget.SetMoney(layerMoneySystem.amount);
    }
}
