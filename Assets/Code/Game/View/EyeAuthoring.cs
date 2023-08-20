using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private Transform m_Transform;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var eye = ref ecsWorld.GetPool<EyeComponent>().AddOrGet(entity);
        eye.camera = m_Camera;
        eye.transform = m_Transform;
        eye.fieldOfView = m_Camera.fieldOfView;   
    }
}
