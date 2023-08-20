using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class EntityAuthoring : SerializedMonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private IEntityComponent[] m_Components = new IEntityComponent[0]; 

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        for (int i = 0; i < m_Components.Length; i++)
        {
            var type = m_Components[i].GetType();
            ecsWorld.GetPoolByType(type).AddRaw(entity, m_Components[i]);
        }
    }
}
