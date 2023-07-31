using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameObjectComponent
{
    public GameObject gameObject;
}

public class GameObjectAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private GameObject m_GameObjectLink;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var gameObjectComponent = ref ecsWorld.GetPool<GameObjectComponent>().Add(entity);
        gameObjectComponent.gameObject = m_GameObjectLink ? m_GameObjectLink : gameObject;
    }
}
