using Leopotam.EcsLite;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering.Universal;

public struct ViewComponent : IEntityComponent
{
    public Vector3 position;
    public Quaternion rotation;
    public float fieldOfView;
}

public class ViewAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_FieldOfView = 60;

    public float fieldOfView => m_FieldOfView;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var viewComponent = ref ecsWorld.GetPool<ViewComponent>().Add(entity);
        viewComponent.position = transform.position;
        viewComponent.rotation = transform.rotation;
        viewComponent.fieldOfView = m_FieldOfView;
    }
}
