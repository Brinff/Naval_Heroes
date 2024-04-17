using Game.UI;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Code.Services;
using UnityEngine;

public class PlayerAbilityTargetIndicatorSystem : MonoBehaviour, IEcsRunSystem, IEcsInitSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_Filter;
    private EcsPool<Ability> m_PoolAbility;
    private EcsPool<AbilityTargetArea> m_PoolAbilityTargetArea;
    private AbilityTargetWidget m_AbilityTargetWidget;
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private AbilityData m_TorpedoAbility;
    [SerializeField]
    private AbilityData m_PlanesAbility;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<Ability>().Inc<AbilityTargetArea>().End();
        m_PoolAbility = m_World.GetPool<Ability>();
        m_PoolAbilityTargetArea = m_World.GetPool<AbilityTargetArea>();


        m_AbilityTargetWidget = ServiceLocator.Get<UIService>().GetElement<AbilityTargetWidget>();
        m_AbilityTargetWidget.SetWorldCamera(m_Camera);
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var ability = ref m_PoolAbility.Get(entity);
            ref var abilityTargetArea = ref m_PoolAbilityTargetArea.Get(entity);
            if (abilityTargetArea.element == null)
            {
                if (ability.id == m_TorpedoAbility.id)
                {
                    abilityTargetArea.time = 3f;
                    abilityTargetArea.element = m_AbilityTargetWidget.CreateTorpedo(abilityTargetArea.point);
                }
                if (ability.id == m_PlanesAbility.id)
                {
                    abilityTargetArea.time = 6f;
                    abilityTargetArea.element = m_AbilityTargetWidget.CreatePlane(abilityTargetArea.point);
                }
                
            }
            else
            {
                m_AbilityTargetWidget.UpdatePosition(abilityTargetArea.element.transform as RectTransform, abilityTargetArea.point);
                if(abilityTargetArea.time > 0)
                {
                    abilityTargetArea.time -= Time.deltaTime;
                }
                else
                {
                    m_AbilityTargetWidget.Hide(abilityTargetArea.element);
                    m_World.DelEntity(entity);
                    
                }
                
            }
        }
    }
}
