using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyeSystem : MonoBehaviour, IEcsInitSystem,  IEcsRunSystem, IEcsGroup<Update>
{
    [SerializeField]
    private GameObject m_Eye;

    private EcsFilter m_Filter;
    private EcsPool<EyeComponent> m_PoolEye;
    private EcsPool<ViewComponent> m_PoolView;
    private EcsWorld m_World;

    [SerializeField]
    private float m_SmoothSpeed = 10;

    public void Init(IEcsSystems systems)
    {
        m_World = systems.GetWorld();
        var entity = m_World.Bake(m_Eye);
        m_World.GetPool<PlayerTag>().AddOrGet(entity);

        m_Filter = m_World.Filter<EyeComponent>().End();
        m_PoolEye = m_World.GetPool<EyeComponent>();
        m_PoolView = m_World.GetPool<ViewComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in m_Filter)
        {
            ref var eye = ref m_PoolEye.Get(entity);
            if (eye.view.Unpack(m_World, out int viewEntity))
            {
                ref var view = ref m_PoolView.Get(viewEntity);
                eye.position = view.position;
                eye.rotation = view.rotation;
                eye.fieldOfView = view.fieldOfView;
            }
            eye.transform.position = Vector3.Lerp(eye.transform.position, eye.position, Time.fixedDeltaTime * m_SmoothSpeed);
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eye.rotation, Time.fixedDeltaTime * m_SmoothSpeed);
            eye.camera.fieldOfView = Mathf.Lerp(eye.camera.fieldOfView, eye.fieldOfView, Time.fixedDeltaTime * m_SmoothSpeed);
        }
    }
}
