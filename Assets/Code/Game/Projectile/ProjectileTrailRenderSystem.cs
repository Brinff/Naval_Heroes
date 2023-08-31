using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct ProjectileTrailRenderer
{
    public bool isRender;
    public int id;
    public GameObject gameObject;
    public Transform transform;
}

public class ProjectileTrailRenderSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroup<Update>
{
    private EcsFilter m_Filter;
    private EcsFilter m_DestroyFilter;

    private EcsPool<ProjectileTrailRenderer> m_PoolTrailRenderer;
    //private EcsPool<ProjectileUnityTransform> m_PoolUnityTransform;
    private EcsPool<ProjectileTransform> m_PoolTransform;

    [SerializeField]
    private GameObject[] m_ProjectilePrefabs = new GameObject[0];

    public void Init(IEcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        m_Filter = ecsWorld.Filter<ProjectileTrailRenderer>().Inc<ProjectileTransform>().End();

        m_DestroyFilter = ecsWorld.Filter<ProjectileTrailRenderer>().Inc<ProjectileDestroyEvent>().End();

        m_PoolTrailRenderer = ecsWorld.GetPool<ProjectileTrailRenderer>();
        m_PoolTransform = ecsWorld.GetPool<ProjectileTransform>();
    }

    //[SerializeField]
    //private List<TrailRenderer> m_TrailsToDestory = new List<TrailRenderer>();
    private Transform m_ProjectileRoot;
    public void Run(IEcsSystems systems)
    {
        if (m_ProjectileRoot == null)
        {
            GameObject projectileRoot = new GameObject("[Projectile Root]");
            m_ProjectileRoot = projectileRoot.transform;
        }

        foreach (var item in m_Filter)
        {
            ref var trailRenderer = ref m_PoolTrailRenderer.Get(item);
            var transform = m_PoolTransform.Get(item);
            if (trailRenderer.gameObject == null)
            {
                var go = Instantiate(m_ProjectilePrefabs[trailRenderer.id], m_ProjectileRoot);
                trailRenderer.gameObject = go;
                trailRenderer.transform = go.transform;
            }

            trailRenderer.transform.position = transform.position;
            trailRenderer.transform.rotation = transform.rotation;
        }

        foreach (var item in m_DestroyFilter)
        {
            var trailRenderer = m_PoolTrailRenderer.Get(item);
            var unityTrailRenderer = trailRenderer.gameObject.GetComponent<TrailRenderer>();
            if (unityTrailRenderer != null) Destroy(trailRenderer.gameObject, unityTrailRenderer.time);
            else Destroy(trailRenderer.gameObject, 10);
            m_PoolTrailRenderer.Del(item);
        }
    }
}
