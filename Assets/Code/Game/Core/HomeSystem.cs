using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSystem : MonoBehaviour
{

    //private EcsWorld m_World;
    //private EcsFilter m_Filter;
    //public void Init(IEcsSystems systems)
    //{
    //    m_World = systems.GetWorld();
    //    m_Filter = m_World.Filter<GoHomeEvent>().Inc<PlayerTag>().End();
    //}

    //public void Run(IEcsSystems systems)
    //{
    //    foreach (var entity in m_Filter)
    //    {
    //        UISystem.Instance.compositionModule.Show<UIHomeComposition>();
    //        UISystem.Instance.GetElement<StartGameWidget>().SetLevel(systems.GetShared<SharedData>().Get<PlayerLevelProvider>().level);
    //        UISystem.Instance.GetElement<SoftMoneyCounterWidget>().SetMoney(systems.GetShared<SharedData>().Get<PlayerMoneySoftProvider>().amount);

    //        m_World.GetPool<GoHomeEvent>().Del(entity);
    //        m_World.GetPool<HomeViewActiveEvent>().Add(entity);
    //        m_World.GetPool<PlayerAimPointComponent>().Del(entity);
    //    }
    //}
}
