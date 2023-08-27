using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGroupAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private bool m_IsSeparetly = false;

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        
    }
}
