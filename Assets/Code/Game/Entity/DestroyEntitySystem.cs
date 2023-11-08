using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEntitySystem : MonoBehaviour, IEcsPostRunSystem, IEcsInitSystem, IEcsGroup<Update>
{
    private EcsWorld m_World;
    private EcsFilter m_FilterA;
    private EcsFilter m_FilterB;
    private EcsFilter m_FilterD;
    private EcsFilter m_FilterC;

    private EcsPool<DestroyComponent> m_DestroyEntity;
    private EcsPool<Childs> m_Childs;
    private EcsPool<GameObjectComponent> m_GameObject;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        m_FilterA = m_World.Filter<DestroyComponent>().End();
        m_FilterB = m_World.Filter<DestroyComponent>().Inc<Childs>().End();
        m_FilterD = m_World.Filter<DestroyComponent>().Inc<GameObjectComponent>().End();

        m_DestroyEntity = m_World.GetPool<DestroyComponent>();
        m_Childs = m_World.GetPool<Childs>();
        m_GameObject = m_World.GetPool<GameObjectComponent>();
    }

    public void PostRun(IEcsSystems systems)
    {
        foreach (var entity in m_FilterA)
        {
            ref var destroyEntity = ref m_DestroyEntity.Get(entity);
            destroyEntity.delay -= Time.deltaTime;
        }

        foreach (var entity in m_FilterB)
        {
            ref var destroyEntity = ref m_DestroyEntity.Get(entity);
            ref var childs = ref m_Childs.Get(entity);
            if (destroyEntity.delay <= 0)
            {
                foreach (var child in childs.entities)
                {
                    if (child.Unpack(m_World, out int childEntity))
                    {
                        //Debug.Log($"DestoryEntity: {childEntity}");
                        m_World.DelEntity(childEntity);
                    }
                }
            }
        }

        foreach (var entity in m_FilterD)
        {
            ref var destroyEntity = ref m_DestroyEntity.Get(entity);
            ref var gameObject = ref m_GameObject.Get(entity);
            if (destroyEntity.delay <= 0)
            {
                //Debug.Log($"DestoryGameObject: {gameObject.gameObject}");
                Destroy(gameObject.gameObject);
            }
        }



        foreach (var entity in m_FilterA)
        {
            ref var destroyEntity = ref m_DestroyEntity.Get(entity);
            if(destroyEntity.delay <= 0)
            {
                m_World.DelEntity(entity);
            }
        }
    }
}
