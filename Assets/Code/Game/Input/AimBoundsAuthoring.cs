using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBoundsAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Transform;
    [SerializeField]
    private Bounds m_Bounds;
    [SerializeField]
    private float m_EnterBoundsExpand = 0;
    [SerializeField]
    private float m_ExitBoundsExpand = 0;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var aimBounds = ref ecsWorld.GetPool<AimBoundsComponent>().Add(entity);
        aimBounds.bounds = m_Bounds;
        aimBounds.transform = m_Transform ? m_Transform : transform;
        aimBounds.position = aimBounds.transform.position;
        aimBounds.enterBoundsExpand = m_EnterBoundsExpand;
        aimBounds.exitBoundsExpand = m_ExitBoundsExpand;
    }

    private void OnDrawGizmosSelected()
    {

        using (new GizmosScope(m_Transform ? m_Transform.localToWorldMatrix : transform.localToWorldMatrix))
        {
            Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);

            Bounds enterBounds = new Bounds(m_Bounds.center, m_Bounds.size);
            enterBounds.Expand(m_EnterBoundsExpand);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(enterBounds.center, enterBounds.size);

            Bounds exitBounds = new Bounds(m_Bounds.center, m_Bounds.size);
            exitBounds.Expand(m_ExitBoundsExpand);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(exitBounds.center, exitBounds.size);
        }
    }
}
