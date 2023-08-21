using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassificationAuhtoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private ClassificationData m_ClassData;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var classEntity = ref ecsWorld.GetPool<Classification>().Add(entity);
        classEntity.id = m_ClassData.id;
    }
}
