using Game.UI;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayerFireSystem : MonoBehaviour, IEcsInitSystem, IEcsRunSystem, IEcsGroupUpdateSystem, IEcsDestroySystem
{
    private FireInputWidget m_InputFireWidget;

    private bool m_IsFire;
    private EcsFilter m_FilterTurretOthers;
    private EcsFilter m_FilterTurretPlayer;
    private EcsWorld m_World;
    private EcsFilter m_FilterFire;
    private EcsPool<WeaponFireCompoment> m_PoolTurretFireCompoment;
    private EcsPool<Root> m_PoolRootComponent;
    private EcsPool<PlayerTag> m_PoolPlayerTag;

    [Button]
    public void Fire(bool isFire)
    {
        m_IsFire = isFire;
    }

    private void OnEnable()
    {

    }

    public void Init(IEcsSystems systems)
    {
        m_InputFireWidget = UISystem.Instance.GetElement<FireInputWidget>();
        m_InputFireWidget.OnPerform += Fire;

        m_World = systems.GetWorld();

        m_FilterTurretPlayer = m_World.Filter<WeaponFireCompoment>().Inc<PlayerTag>().Exc<AITag>().End();
        m_FilterTurretOthers = m_World.Filter<WeaponFireCompoment>().Inc<Root>().Exc<PlayerTag>().End();

        m_PoolTurretFireCompoment = m_World.GetPool<WeaponFireCompoment>();
        m_PoolRootComponent = m_World.GetPool<Root>();
        m_PoolPlayerTag = m_World.GetPool<PlayerTag>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var item in m_FilterTurretOthers)
        {
            if (m_PoolRootComponent.Has(item))
            {
                ref var rootComponent = ref m_PoolRootComponent.Get(item);

                if (rootComponent.entity.Unpack(m_World, out int rootEntity))
                {
                    if (m_PoolPlayerTag.Has(rootEntity)) m_PoolPlayerTag.Add(item);
                }
            }
        }

        foreach (var entity in m_FilterTurretPlayer)
        {
            ref var turretFireCompoment = ref m_PoolTurretFireCompoment.Get(entity);
            turretFireCompoment.isFire = m_IsFire;
        }
    }

    public void Destroy(IEcsSystems systems)
    {
        if (m_InputFireWidget) m_InputFireWidget.OnPerform -= Fire;
    }

    //#if UNITY_EDITOR
    //    private void Update()
    //    {
    //        //if (Input.GetMouseButtonDown(0))
    //        //{
    //        //    Fire(true);
    //        //}

    //        //if (Input.GetMouseButtonUp(0))
    //        //{
    //        //    Fire(false);
    //        //}
    //    }


    //#endif
}
