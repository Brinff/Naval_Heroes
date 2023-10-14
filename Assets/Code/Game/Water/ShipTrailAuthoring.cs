using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrailAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Orgin;
    [SerializeField]
    private float m_Lifetime = 10;
    [SerializeField]
    private Vector3 m_Scale = Vector3.one;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var shipTrail = ref ecsWorld.GetPool<ShipTrail>().Add(entity);
        shipTrail.scale = m_Scale;
        shipTrail.lifetime = m_Lifetime;
        shipTrail.orgin = m_Orgin;
    }
}
