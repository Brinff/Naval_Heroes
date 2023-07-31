using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;


public struct ProjectileUnityTransform
{
    public Transform transform;
}

public struct ProjectileTransform
{
    public Vector3 position;
    public Quaternion rotation;
}

public struct ProjectilePhysic
{
    public float time;
    public float timeFactor;
    public Vector3 velocity;
    public float drag;
}

public class ProjectilePhysicSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsFilter m_DestroyFilter;
    private EcsPool<ProjectilePhysic> m_PoolsPhysic;
    private EcsPool<ProjectileTransform> m_PoolsTransform;
    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        m_Filter = world.Filter<ProjectilePhysic>().Inc<ProjectileTransform>().End();
        //m_DestroyFilter = world.Filter<ProjectilePhysic>().Inc<ProjectileDestroyEvent>().End();
        m_PoolsPhysic = world.GetPool<ProjectilePhysic>();
        m_PoolsTransform = world.GetPool<ProjectileTransform>();
    }

    public void Run(IEcsSystems systems)
    {
        float dt = Time.deltaTime;
       
        foreach (int entity in m_Filter)
        {

            ref ProjectilePhysic physic = ref m_PoolsPhysic.Get(entity);
            ref ProjectileTransform transform = ref m_PoolsTransform.Get(entity);

            float scaleDt = dt * physic.timeFactor;

            physic.velocity += Physics.gravity * scaleDt;

            if (physic.drag > 0)
            {
                physic.velocity *= Mathf.Clamp01(1f - physic.drag * scaleDt);
            }
            physic.time += scaleDt * physic.timeFactor;
            transform.position += physic.velocity * scaleDt;
            transform.rotation = Quaternion.LookRotation(Vector3.Normalize(physic.velocity));
        }

        //foreach (int entity in m_DestroyFilter)
        //{
        //    //Debug.Log(m_PoolsPhysic.Get(entity).time);
        //    m_PoolsPhysic.Del(entity);
        //}
    }
}
