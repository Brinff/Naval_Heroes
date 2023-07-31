using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FireComponent
{
    public List<int> turrets;
}

public class FireAuthoring : MonoBehaviour, IEntityAuthoring
{
    public bool isEnable => gameObject.activeInHierarchy;

    //private Turret[] m_Turrets = null;

    //public delegate void TurretBehaviour(Turret turret);

    //public void ForEach(TurretBehaviour turretBehaviour)
    //{
    //    m_Turrets = GetComponentsInChildren<Turret>();
    //    foreach (Turret t in m_Turrets)
    //    {
    //        turretBehaviour?.Invoke(t);
    //    }
    //}

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ecsWorld.GetPool<FireComponent>().Add(entity);
    }
}