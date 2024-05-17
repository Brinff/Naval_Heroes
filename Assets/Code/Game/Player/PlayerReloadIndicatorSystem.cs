using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public struct WeaponReloadIndicatorComponent
{
    public ReloadIndicatorItem indicator;
}

public class PlayerReloadIndicatorSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsWorld m_World;
    private EcsFilter m_FilterNew;
    private EcsFilter m_Filter;
    private EcsFilter m_FilterDestory;
    private EcsPool<WeaponReloadComponent> m_WeaponReload;
    private EcsPool<WeaponReloadIndicatorComponent> m_WeaponReloadIndicator;
    private EcsPool<Childs> m_Childs;
    private ReloadIndicatorWidget m_ReloadIndicatorWidget;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_FilterNew = m_World.Filter<PlayerTag>().Inc<WeaponReloadComponent>().Exc<WeaponReloadIndicatorComponent>().Exc<AITag>().End();
        m_Filter = m_World.Filter<PlayerTag>().Inc<WeaponReloadComponent>().Inc<WeaponReloadIndicatorComponent>().Exc<AITag>().End();
        m_FilterDestory = m_World.Filter<PlayerTag>().Inc<DestroyComponent>().Inc<Childs>().End();
        m_WeaponReload = m_World.GetPool<WeaponReloadComponent>();
        m_WeaponReloadIndicator = m_World.GetPool<WeaponReloadIndicatorComponent>();
        m_Childs = m_World.GetPool<Childs>();
        m_ReloadIndicatorWidget = ServiceLocator.Get<UIController>().GetWidget<ReloadIndicatorWidget>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_FilterNew)
        {
            ref var weaponReloadIndicator = ref m_WeaponReloadIndicator.Add(entity);
            ref var weaponReload = ref m_WeaponReload.Get(entity);
            weaponReloadIndicator.indicator = m_ReloadIndicatorWidget.CreateIndicator(weaponReload.progress);
        }

        foreach (var entity in m_Filter)
        {
            ref var weaponReloadIndicator = ref m_WeaponReloadIndicator.Get(entity);
            ref var weaponReload = ref m_WeaponReload.Get(entity);
            weaponReloadIndicator.indicator.SetProgress(weaponReload.progress);
        }

        foreach (var entity in m_FilterDestory)
        {
            ref var childs = ref m_Childs.Get(entity);
            foreach (var item in childs.entities)
            {
                if(item.Unpack(m_World, out int childEntity))
                {
                    if (m_WeaponReloadIndicator.Has(childEntity))
                    {
                        ref var weaponReloadIndicator = ref m_WeaponReloadIndicator.Get(childEntity);
                        m_ReloadIndicatorWidget.DestoryIndicator(weaponReloadIndicator.indicator);
                        m_WeaponReloadIndicator.Del(childEntity);
                    }
                }
            }
            
        }
    }
}
