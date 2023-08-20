using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyAddCommand : MonoBehaviour, ICommand<int>
{
    public void Execute(EcsWorld world, IEcsSystems systems, int amount)
    {
        SoftMoneyCounterWidget softMoneyCounterWidget = UISystem.Instance.GetElement<SoftMoneyCounterWidget>();
        PlayerMoneySystem playerMoneySystem = systems.GetSystem<PlayerMoneySystem>();
        playerMoneySystem.AddMoney(amount);
        softMoneyCounterWidget.SetMoney(playerMoneySystem.amount);
    }
}
