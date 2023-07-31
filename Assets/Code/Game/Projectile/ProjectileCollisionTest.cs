using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileCollisionTest : MonoBehaviour, IProjectileRaycastEnter
{
    private Collider m_Collider;
    private void OnEnable()
    {
        m_Collider = GetComponent<Collider>();
        ProjectileRaycastSystem.RegisterCollider(m_Collider);
    }

    [SerializeField]
    private VisualEffect m_VisualEffect;

    private void OnDisable()
    {
        ProjectileRaycastSystem.UnregisterCollider(m_Collider);
    }

    public void OnProjectileRaycastEnter(EcsWorld world, int enitity, RaycastHit hit)
    {
        m_VisualEffect.transform.position = hit.point;
        m_VisualEffect.Play();
    }
}
