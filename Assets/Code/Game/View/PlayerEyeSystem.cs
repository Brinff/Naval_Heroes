using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyeSystem : MonoBehaviour, IEcsInitSystem, IEcsGroupFixedUpdateSystem, IEcsRunSystem
{
    [SerializeField]
    private GameObject m_Eye;

    private EcsFilter m_Filter;
    private EcsPool<EyeComponent> m_PoolEye;


    [SerializeField]
    private float m_SmoothSpeed = 10;

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        var entity = world.Bake(m_Eye);
        world.GetPool<PlayerTagLeo>().AddOrGet(entity);

        m_Filter = world.Filter<EyeComponent>().End();
        m_PoolEye = world.GetPool<EyeComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var eye = ref m_PoolEye.Get(entity);
            eye.transform.position = Vector3.Lerp(eye.transform.position, eye.position, Time.fixedDeltaTime * m_SmoothSpeed);
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eye.rotation, Time.fixedDeltaTime * m_SmoothSpeed);
            eye.camera.fieldOfView = Mathf.Lerp(eye.camera.fieldOfView, eye.fieldOfView, Time.fixedDeltaTime * m_SmoothSpeed);
        }
    }
}
