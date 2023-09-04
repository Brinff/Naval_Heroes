using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;
using UnityEngine.Splines;
using Unity.Mathematics;
using Warships;

public class PlaneMoveSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsWorld m_World;
    private EcsPool<MovePath> m_PoolMovePath;
    private EcsPool<TransformComponent> m_PoolTransform;
    private EcsPool<DestroyComponent> m_PoolDestroy;
    private EcsPool<Team> m_PoolTeam;


    public float speedPlane = 10;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_Filter = m_World.Filter<PlaneTag>().Inc<MovePath>().Inc<TransformComponent>().Exc<DestroyComponent>().End();
        m_PoolMovePath = m_World.GetPool<MovePath>();
        m_PoolTransform = m_World.GetPool<TransformComponent>();
        m_PoolDestroy = m_World.GetPool<DestroyComponent>();
        m_PoolTeam = m_World.GetPool<Team>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var movePath = ref m_PoolMovePath.Get(entity);
            ref var transform = ref m_PoolTransform.Get(entity);
            if (movePath.spline != null)
            {
                float lenght = movePath.spline.GetLength();
                float t = movePath.position / lenght;
                if (movePath.spline.Evaluate(movePath.position / lenght, out float3 position, out float3 tangent, out float3 uvVector))
                {
                    transform.transform.position = position;
                    transform.transform.rotation = Quaternion.LookRotation(tangent, uvVector);
                    movePath.position += Time.deltaTime * speedPlane;

                    if (Vector3.Distance(movePath.target, new Vector3(position.x, movePath.target.y, position.z)) < 50 && !movePath.isFire)
                    {
                        ref var team = ref m_PoolTeam.Get(entity);
                        Vector3 direction = Ballistics.GetDirection(transform.transform.position, movePath.target, 0, 50); 
                        CannonProjectile cannonProjectile = new CannonProjectile() { damage = movePath.damage, team = team.id, timeFactor = 1, velocity = 50 };
                        cannonProjectile.Launch(m_World, transform.transform.position, direction /*Vector3.Normalize(movePath.target - transform.transform.position)*/);
                        movePath.isFire = true;
                    }
                }

                if (movePath.position > lenght)
                {
                    m_PoolDestroy.Add(entity);
                }
            }
        }
    }
}
