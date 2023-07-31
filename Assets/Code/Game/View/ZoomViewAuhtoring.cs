using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomViewAuhtoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Height;
    [SerializeField]
    private Transform m_Transform;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var zoomView = ref ecsWorld.GetPool<ZoomViewComponent>().AddOrGet(entity);
        zoomView.height = m_Height;
        zoomView.transform = m_Transform ? m_Transform : transform;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + Vector3.up * m_Height;
        Gizmos.DrawSphere(position, 1);
    }
}
