using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InfoComponent
{
    public Transform orgin;
}

public class InfoAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Orign;

    public Transform orgin => m_Orign;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var infoComponent = ref ecsWorld.GetPool<InfoComponent>().Add(entity);
        infoComponent.orgin = m_Orign;
    }
}
