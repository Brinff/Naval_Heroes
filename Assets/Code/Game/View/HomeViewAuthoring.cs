using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeViewAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;
    [SerializeField]
    private Transform m_Orgin;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var homeView = ref ecsWorld.GetPool<HomeViewComponent>().Add(entity);
        homeView.orgin = m_Orgin;
    }
}
