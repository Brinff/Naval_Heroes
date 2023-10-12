using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrailAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Orgin;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var shipTrail = ref ecsWorld.GetPool<ShipTrail>().Add(entity);
        shipTrail.orgin = m_Orgin;
    }
}
