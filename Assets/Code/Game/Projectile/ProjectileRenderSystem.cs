using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct ProjectileRenderer
{
    public bool isRender;
    public GameObject gameObject;
    public Transform transform;
}

public class ProjectileRenderSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem
{
    private EcsFilter m_Filter;
    private EcsFilter m_DestroyFilter;
    private EcsPool<ProjectileRenderer> m_PoolRenderer;
    //private EcsPool<ProjectileUnityTransform> m_PoolUnityTransform;
    private EcsPool<ProjectileTransform> m_PoolTransform;

    [SerializeField]
    private GameObject m_ProjectilePrefab;

    private Transform m_ProjectileRoot;

    public void Init(IEcsSystems systems)
    {
        EcsWorld ecsWorld = systems.GetWorld();
        m_Filter = ecsWorld.Filter<ProjectileRenderer>().Inc<ProjectileTransform>().End();
        m_PoolRenderer = ecsWorld.GetPool<ProjectileRenderer>();
        m_PoolTransform = ecsWorld.GetPool<ProjectileTransform>();

        m_DestroyFilter = ecsWorld.Filter<ProjectileRenderer>().Inc<ProjectileDestroyEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (m_ProjectileRoot == null)
        {
            GameObject projectileRoot = new GameObject("[Projectile Root]");
            m_ProjectileRoot = projectileRoot.transform;
        }

        foreach (var item in m_Filter)
        {
            ref var renderer = ref m_PoolRenderer.Get(item);
            var transform = m_PoolTransform.Get(item);
            if (renderer.gameObject == null)
            {
                var go = Instantiate(m_ProjectilePrefab, m_ProjectileRoot);
                renderer.gameObject = go;
                renderer.transform = go.transform;
            }

            renderer.transform.position = transform.position;
            renderer.transform.rotation = transform.rotation;
        }

        foreach (var item in m_DestroyFilter)
        {
            var renderer = m_PoolRenderer.Get(item);
            Destroy(renderer.gameObject);
            m_PoolRenderer.Del(item);
        }
    }
}
