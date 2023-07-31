using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtViewAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Transform;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var lookAtView = ref ecsWorld.GetPool<LookAtViewComponent>().Add(entity);
        lookAtView.transform = m_Transform ? m_Transform : this.transform;
        lookAtView.rotation = Quaternion.identity;
        lookAtView.distance = 1000;
        lookAtView.position = lookAtView.rotation * Vector3.forward * lookAtView.distance;
    }
}
