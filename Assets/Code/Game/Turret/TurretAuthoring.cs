using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TurretComponent
{

}

public struct TurretIdleComponent
{

}
public enum AimState
{
    Idle, Aim
}

public struct TurretAimComponent
{
    public AimState state;
    public Vector3 target;
}

public struct WeaponFireCompoment
{
    public bool isFire;
    public bool isActive;
    public float delay;
    public bool isFireNow;
}

public class TurretAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<TurretComponent>().Add(entity);
        ecsWorld.GetPool<WeaponFireCompoment>().Add(entity);
        ecsWorld.GetPool<TurretAimComponent>().Add(entity);
    }
}