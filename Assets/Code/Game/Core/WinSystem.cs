using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    private EcsFilter m_Filter;
    private EcsFilter m_RewardFilter;
    private EcsWorld m_World;
    private WinWidget m_WinWidget;

    public void Destroy(IEcsSystems systems)
    {
        if (m_WinWidget) m_WinWidget.OnClaim -= OnClaim;
    }


    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<GoWinEvent>().End();
        m_RewardFilter = m_World.Filter<RewardComponent>().End();
        m_WinWidget = UISystem.Instance.GetElement<WinWidget>();
        m_WinWidget.OnClaim += OnClaim;
    }

    private bool m_IsClaimTrigger;

    private void OnClaim()
    {
        m_IsClaimTrigger = true;
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_RewardFilter)
        {
            if (m_IsClaimTrigger)
            {
                ref var reward = ref m_World.GetPool<RewardComponent>().Get(entity);

                PlayerMoneySoftProvider playerMoneySoftProvider = systems.GetShared<SharedData>().Get<PlayerMoneySoftProvider>();
                playerMoneySoftProvider.AddMoney(reward.amount);

                m_World.GetPool<GoHomeEvent>().Add(entity);
                m_World.GetPool<RewardComponent>().Del(entity);
                m_IsClaimTrigger = false;
            }
        }
        foreach (var entity in m_Filter)
        {
            ref var reward = ref m_World.GetPool<RewardComponent>().AddOrGet(entity);
            ref var level = ref m_World.GetPool<LevelComponent>().Get(entity);

            reward.amount = level.data.reward;

            PlayerLevelProvider playerLevelProvider = systems.GetShared<SharedData>().Get<PlayerLevelProvider>();

            m_WinWidget.SetReward(reward.amount, false);
            m_WinWidget.SetLevel(playerLevelProvider.level);
            playerLevelProvider.CompleteLevel();

            UISystem.Instance.compositionModule.Show<UIWinCompositon>();
            m_World.GetPool<LevelComponent>().Del(entity);
            m_World.GetPool<GoWinEvent>().Del(entity);
        }
    }
}
