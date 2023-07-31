using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AISearchRadiusAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_Radius = 100;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var searchRadius = ref ecsWorld.GetPool<AISearchRadiusComponent>().AddOrGet(entity);
        searchRadius.value = m_Radius;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        using(new Handles.DrawingScope(Color.red, transform.localToWorldMatrix))
        {
            Handles.CircleHandleCap(0, Vector3.zero, Quaternion.LookRotation(Vector3.up), m_Radius, EventType.Repaint);
        }    
    }
#endif

}
