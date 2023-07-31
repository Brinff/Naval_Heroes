using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR

using UnityEditor;

#endif
using UnityEngine;

public class AISearchAngleAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField, MinMaxSlider(-180, 180, true)]
    private Vector2 m_AngleSearch = new Vector2(-180, 180);

    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var searchAngle = ref ecsWorld.GetPool<AISearchAngleComponent>().AddOrGet(entity);
        searchAngle.min = m_AngleSearch.x;
        searchAngle.max = m_AngleSearch.y;
    }
#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        using (new Handles.DrawingScope(new Color(1,0,0,0.1f), transform.localToWorldMatrix))
        {
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, -m_AngleSearch.x, 30, 2);
            Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.forward, -m_AngleSearch.y, 30, 2);

            Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, -m_AngleSearch.x, 30);
            Handles.DrawSolidArc(Vector3.zero, Vector3.up, Vector3.forward, -m_AngleSearch.y, 30);


        }
    }
#endif
}
