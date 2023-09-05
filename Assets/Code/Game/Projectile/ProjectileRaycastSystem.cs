using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct ProjectileHit
{
    public float distance;
    public Vector3 normal;
    public Vector3 point;
    public Vector3 direction;
    public EcsPackedEntity colliderEntity;
    public EcsPackedEntity projectileEntity;
}

public struct ProjectileOwnerComponent
{
    public EcsPackedEntity entity;
}

public struct ProjectileRaycast
{
    public Vector3 previusPosition;
    public Dictionary<Collider, ProjectileHit> hits;
}

public interface IProjectileRaycast
{

}

public interface IProjectileRaycastEnter : IProjectileRaycast
{
    void OnProjectileRaycastEnter(EcsWorld world, int enitity, RaycastHit hit);
}

public interface IProjectileRaycastExit : IProjectileRaycast
{
    void OnProjectileRaycastExit(EcsWorld world, int enitity, RaycastHit hit);
}

public class ProjectileRaycastSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_FilterProjectile;
    private EcsFilter m_FilterCollider;
    private EcsPool<ProjectileRaycast> m_PoolRaycast;
    private EcsPool<ProjectileTransform> m_PoolTransform;
    private EcsPool<ProjectileColliderComponent> m_PoolProjectileColliderComponent;
    private EcsPool<RootComponent> m_PoolRootComponent;
    private EcsPool<Team> m_PoolTeam;
    private EcsPool<ProjectileColliderHitComponent> m_PoolProjectileColliderHitComponent;

    private EcsWorld m_World;

    private EcsFilter m_DestroyFilter;

    private static List<Collider> s_Colliders = new List<Collider>();

    public static void RegisterCollider(Collider collider)
    {
        if (s_Colliders.Contains(collider)) return;
        s_Colliders.Add(collider);
    }

    public static void UnregisterCollider(Collider collider)
    {
        s_Colliders.Remove(collider);
    }

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_FilterProjectile = m_World.Filter<ProjectileRaycast>().Inc<ProjectileTransform>().End();
        m_FilterCollider = m_World.Filter<ProjectileColliderComponent>().Inc<RootComponent>().End();
        m_DestroyFilter = m_World.Filter<ProjectileRaycast>().Inc<ProjectileDestroyEvent>().End();

        m_PoolRaycast = m_World.GetPool<ProjectileRaycast>();
        m_PoolTransform = m_World.GetPool<ProjectileTransform>();
        m_PoolProjectileColliderComponent = m_World.GetPool<ProjectileColliderComponent>();
        m_PoolTeam = m_World.GetPool<Team>();
        m_PoolRootComponent = m_World.GetPool<RootComponent>();
        m_PoolProjectileColliderHitComponent = m_World.GetPool<ProjectileColliderHitComponent>();
    }

    //private List<Tuple<Transform, Vector3>> m_Points = new List<Tuple<Transform, Vector3>>();

    //private void OnDrawGizmos()
    //{
    //    foreach (var item in m_Points)
    //    {

    //        Gizmos.DrawSphere(item.Item1.transform.TransformPoint(item.Item2), 0.5f);
    //    }
    //}

    //[Button]
    //private void ClearPoints()
    //{
    //    m_Points.Clear();
    //}

    //private void AddPoint(RaycastHit hit)
    //{
    //    m_Points.Add(new Tuple<Transform, Vector3>(hit.transform, hit.transform.InverseTransformPoint(hit.point)));
    //}


    //private Dictionary<int, HashSet<Collider>> m_ProjectileRaycast = new Dictionary<int, HashSet<Collider>>();

    public void Run(IEcsSystems systems)
    {
        Ray ray = new Ray();
        foreach (var projectileEntity in m_FilterProjectile)
        {
            ref var transform = ref m_PoolTransform.Get(projectileEntity);
            ref var raycast = ref m_PoolRaycast.Get(projectileEntity);
            ref var team = ref m_PoolTeam.Get(projectileEntity);


            Vector3 delta = transform.position - raycast.previusPosition;
            float magintude = delta.magnitude;
            Vector3 direction = Vector3.Normalize(delta);


            ray.direction = direction;
            ray.origin = raycast.previusPosition;

            //Nullable<EcsPackedEntity> ownerEntity = null;
            //if (m_PoolTeam.Has(projectileEntity))
            //{
            //    ref var projectileOwnerComponent = ref m_PoolTeam.Get(projectileEntity);
            //    ownerEntity = projectileOwnerComponent.entity;
            //}



            foreach (var colliderEntity in m_FilterCollider)
            {
                ref var rootComponent = ref m_PoolRootComponent.Get(colliderEntity);

                if(rootComponent.entity.Unpack(m_World, out int rootEntity))
                {
                    if (m_PoolTeam.Has(rootEntity))
                    {
                        ref var colliderTeam = ref m_PoolTeam.Get(rootEntity);
                        if (team.id == colliderTeam.id) continue;
                    }
                }

                //if (ownerEntity != null)
                //{
                //    if (ownerEntity.Value.EqualsTo(in rootComponent.entity)) continue;
                //}

                ref var colliderComponent = ref m_PoolProjectileColliderComponent.Get(colliderEntity);

                var collider = colliderComponent.collider;

                if (raycast.hits == null || !raycast.hits.ContainsKey(collider))
                {
                    if (collider.Raycast(ray, out RaycastHit hit, magintude))
                    {
                        ProjectileHit projectileHit = new ProjectileHit();
                        projectileHit.projectileEntity = m_World.PackEntity(projectileEntity);
                        projectileHit.colliderEntity = m_World.PackEntity(colliderEntity);
                        projectileHit.point = hit.point;
                        projectileHit.normal = hit.normal;
                        projectileHit.distance = hit.distance;
                        projectileHit.direction = ray.direction;
                        ref var projectileColliderHitComponent = ref m_PoolProjectileColliderHitComponent.Get(colliderEntity);
                        if (projectileColliderHitComponent.lenght < 10)
                        {
                            projectileColliderHitComponent.hits[projectileColliderHitComponent.lenght] = projectileHit;
                            projectileColliderHitComponent.lenght++;
                        }

                        if (raycast.hits == null) raycast.hits = new Dictionary<Collider, ProjectileHit>();
                        raycast.hits.Add(collider, projectileHit);
                    }
                }

                //ray.direction = -direction;
                //ray.origin = transform.position;

                //if (raycast.cacheColliders?.Contains(collider) ?? false)
                //{
                //    if (collider.Raycast(ray, out RaycastHit hitBackward, magintude))
                //    {
                //        //Debug.Log($"Exit: {item} collider {hitBackward.collider}");
                //        //var projectileRaycast = collider.GetComponent<IProjectileRaycast>();
                //        //projectileRaycast.As<IProjectileRaycastExit>()?.OnProjectileRaycastExit(m_World, item, hitBackward);
                //        break;
                //    }
                //}
                //else
                //{
                //    if (collider.Raycast(ray, out RaycastHit hitForward, magintude))
                //    {
                //        //Debug.Log($"Enter: {item} collider {hitForward.collider}");

                //        if (raycast.cacheColliders == null) raycast.cacheColliders = new HashSet<Collider>();
                //        raycast.cacheColliders.Add(collider);

                //        //var projectileRaycast = collider.GetComponent<IProjectileRaycast>();

                //        //projectileRaycast.As<IProjectileRaycastEnter>()?.OnProjectileRaycastEnter(m_World, item, hitForward);
                //    }
                //}
            }
            //}
            //else
            //{
            //    ray.direction = -direction;
            //    ray.origin = transform.position;

            //    for (int i = 0; i < countCollider; i++)
            //    {
            //        var collider = s_Colliders[i];
            //        if (collider.Raycast(ray, out RaycastHit hit, magintude))
            //        {
            //            Debug.Log($"Exit: {item} collider {hit.collider}");
            //            m_ProjectileRaycast[item].As<IProjectileRaycastExit>()?.OnProjectileRaycastExit(m_World, item, hit);
            //            m_ProjectileRaycast.Remove(item);
            //        }
            //    }
            //}

            raycast.previusPosition = transform.position;
        }

        //foreach (var item in m_DestroyFilter)
        //{
        //    m_ProjectileRaycast.Remove(item);
        //}
    }
}
