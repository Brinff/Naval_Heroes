using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponFireSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    [SerializeField]
    private float m_FireRange;
    private EcsFilter m_Filter;
    private EcsFilter m_FilterTurretOthers;
    private EcsPool<WeaponFireCompoment> m_PoolWeaponFire;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<AITargetComponent> m_PoolAITarget;
    private EcsPool<Root> m_PoolRoot;
    private EcsPool<AITag> m_PoolAITag;

    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<AITag>().Inc<AITargetComponent>().Inc<WeaponFireCompoment>().Inc<TransformComponent>().End();

        m_FilterTurretOthers = m_World.Filter<WeaponFireCompoment>().Inc<Root>().Exc<AITag>().End();


        m_PoolWeaponFire = m_World.GetPool<WeaponFireCompoment>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolAITarget = m_World.GetPool<AITargetComponent>();
        m_PoolAITag = m_World.GetPool<AITag>();
        m_PoolRoot = m_World.GetPool<Root>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_FilterTurretOthers)
        {
            if (m_PoolRoot.Has(item))
            {
                ref var rootComponent = ref m_PoolRoot.Get(item);

                if (rootComponent.entity.Unpack(m_World, out int rootEntity))
                {
                    if (m_PoolAITag.Has(rootEntity))
                    {
                        //m_PoolAITarget.Add(item);
                        m_PoolAITag.Add(item);
                    }
                }
            }
        }

        foreach (var entity in m_Filter)
        {
            ref var aiTarget = ref m_PoolAITarget.Get(entity);
            if(aiTarget.entity.Unpack(m_World, out int targetEntity))
            {
                ref var targetEntityTransform = ref m_PoolTransform.Get(targetEntity);
                ref var weaponFire = ref m_PoolWeaponFire.Get(entity);
                ref var transform = ref m_PoolTransform.Get(entity);

                float distanceToTarget = Vector3.Distance(transform.transform.position, targetEntityTransform.transform.position);
                
                weaponFire.isFire = distanceToTarget < m_FireRange;
            }
            else
            {
                 ref var weaponFire = ref m_PoolWeaponFire.Get(entity);
                weaponFire.isFire = false;
            }
        }
    }
}
