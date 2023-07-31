using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;


public class TransformAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Transform;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var transform = ref ecsWorld.GetPool<TransformComponent>().Add(entity);
        transform.transform = m_Transform ? m_Transform : this.transform;
    }
}