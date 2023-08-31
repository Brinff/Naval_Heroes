using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;


public class ProjectileMoveToTargetSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsFilter m_DestroyFilter;
    private EcsPool<ProjectileMoveToTarget> m_PoolsPhysic;
    private EcsPool<ProjectileTransform> m_PoolsTransform;
    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        m_Filter = world.Filter<ProjectileMoveToTarget>().Inc<ProjectileTransform>().End();
        //m_DestroyFilter = world.Filter<ProjectilePhysic>().Inc<ProjectileDestroyEvent>().End();
        m_PoolsPhysic = world.GetPool<ProjectileMoveToTarget>();
        m_PoolsTransform = world.GetPool<ProjectileTransform>();
    }

    public void Run(IEcsSystems systems)
    {
        float dt = Time.deltaTime;
       
        foreach (int entity in m_Filter)
        {

            ref ProjectileMoveToTarget moveToTarget = ref m_PoolsPhysic.Get(entity);
            ref ProjectileTransform transform = ref m_PoolsTransform.Get(entity);

            float scaleDt = dt * moveToTarget.timeFactor;
            Vector3 direction = Vector3.Normalize(moveToTarget.target - transform.position);
            moveToTarget.velocity = Vector3.Lerp(moveToTarget.velocity, direction * moveToTarget.speed, scaleDt * moveToTarget.speedLookAt);

            //if (physic.drag > 0)
            //{
            //    physic.velocity *= Mathf.Clamp01(1f - physic.drag * scaleDt);
            //}
            moveToTarget.time += scaleDt * moveToTarget.timeFactor;
            transform.position += moveToTarget.velocity * scaleDt;
            transform.rotation = Quaternion.LookRotation(Vector3.Normalize(moveToTarget.velocity));
        }

        //foreach (int entity in m_DestroyFilter)
        //{
        //    //Debug.Log(m_PoolsPhysic.Get(entity).time);
        //    m_PoolsPhysic.Del(entity);
        //}
    }
}
