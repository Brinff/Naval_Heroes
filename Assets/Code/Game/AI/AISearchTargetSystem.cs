using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISearchTargetSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_FilterWithRoot;
    private EcsFilter m_FilterTargets;
    private EcsFilter m_Filter;
    private EcsFilter m_TeamFilter;
    private EcsPool<AITargetComponent> m_PoolAITarget;
    private EcsPool<RootComponent> m_PoolRoot;
    private EcsPool<TeamComponent> m_PoolTeam;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<AISearchAngleComponent> m_PoolAISearchAngle;
    private EcsPool<AISearchRadiusComponent> m_PoolAISearchRadius;
    private EcsWorld m_World;
    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        //m_FilterTargets = m_World.Filter<AITargetComponent>().End();
        m_FilterWithRoot = m_World.Filter<AITargetComponent>().Inc<TransformComponent>().Inc<RootComponent>().Exc<DeadTag>().End();
        m_Filter = m_World.Filter<AITargetComponent>().Inc<TeamComponent>().Inc<TransformComponent>().Exc<DeadTag>().End();

        m_TeamFilter = m_World.Filter<TeamComponent>().Exc<DeadTag>().End();
        m_PoolAITarget = m_World.GetPool<AITargetComponent>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolRoot = m_World.GetPool<RootComponent>();
        m_PoolTeam = m_World.GetPool<TeamComponent>();
        m_PoolAISearchAngle = m_World.GetPool<AISearchAngleComponent>();
        m_PoolAISearchRadius = m_World.GetPool<AISearchRadiusComponent>();
    }

    public void AITurrets()
    {
        //foreach (var entity in m_FilterTargets)
        //{
        //    ref var aiTarget = ref m_PoolAITarget.Get(entity);
        //    if (aiTarget.entity.Unpack(m_World, out int targetEntity))
        //    {

        //    }
        //}
        foreach (var entity in m_FilterWithRoot)
        {
            ref var root = ref m_PoolRoot.Get(entity);
            if (root.entity.Unpack(m_World, out int rootEntity))
            {
                ref var aiTarget = ref m_PoolAITarget.Get(entity);
                ref var team = ref m_PoolTeam.Get(rootEntity);
                ref var transform = ref m_PoolTransform.Get(entity);
                Nullable<int> findEntity = null;
                float findDistance = 0;

                bool hasAngleSearch = m_PoolAISearchAngle.Has(entity);
                float angleMin = 0;
                float angleMax = 0;
                if (hasAngleSearch)
                {
                    ref var aiSearchAngle = ref m_PoolAISearchAngle.Get(entity);
                    angleMin = aiSearchAngle.min;
                    angleMax = aiSearchAngle.max;
                }

                float radius = 0;
                bool hasSearchRadius = m_PoolAISearchRadius.Has(entity);
                if (hasSearchRadius)
                {
                    ref var aiSearchRadius = ref m_PoolAISearchRadius.Get(entity);
                    radius = aiSearchRadius.value;
                }

                foreach (var targetEntity in m_TeamFilter)
                {
                    ref var targetTeam = ref m_PoolTeam.Get(targetEntity);
                    if (targetTeam.id != team.id)
                    {

                        ref var targetTransform = ref m_PoolTransform.Get(targetEntity);
                        Vector3 delta = targetTransform.transform.position - transform.transform.position;
                        Vector3 direction = Vector3.Normalize(delta);

                        if (hasAngleSearch)
                        {
                            float angle = Vector3.SignedAngle(transform.transform.forward, direction, Vector3.up);
                            if (angle < angleMin || angle > angleMax)
                            {
                                continue;
                            }
                        }

                        float distance = delta.magnitude;

                        if (hasSearchRadius)
                        {
                            if (distance > radius) continue;
                        }
                        
                        if (findEntity != null)
                        {
                            if (findDistance > distance)
                            {
                                findEntity = targetEntity;
                                findDistance = distance;
                            }
                        }
                        else
                        {
                            findEntity = targetEntity;
                            findDistance = distance;
                        }
                    }
                }

                if (findEntity != null) aiTarget.entity = m_World.PackEntity(findEntity.Value);
                else aiTarget.entity = new EcsPackedEntity();
            }
        }
    }

    public void AIShips()
    {
        foreach (var entity in m_Filter)
        {
            ref var aiTarget = ref m_PoolAITarget.Get(entity);
            ref var team = ref m_PoolTeam.Get(entity);
            ref var transform = ref m_PoolTransform.Get(entity);
            Nullable<int> findEntity = null;
            float findDistance = 0;

            foreach (var targetEntity in m_TeamFilter)
            {
                ref var targetTeam = ref m_PoolTeam.Get(targetEntity);
                if (targetTeam.id != team.id)
                {

                    ref var targetTransform = ref m_PoolTransform.Get(targetEntity);
                    float distance = Vector3.Distance(targetTransform.transform.position, transform.transform.position);
                    if (findEntity != null)
                    {
                        if (findDistance > distance)
                        {
                            findEntity = targetEntity;
                            findDistance = distance;
                        }
                    }
                    else
                    {
                        findEntity = targetEntity;
                        findDistance = distance;
                    }
                }
            }

            if (findEntity != null) aiTarget.entity = m_World.PackEntity(findEntity.Value);
            else aiTarget.entity = new EcsPackedEntity();
        }
    }

    public void Run(IEcsSystems systems)
    {
        AITurrets();
        AIShips();
    }
}
