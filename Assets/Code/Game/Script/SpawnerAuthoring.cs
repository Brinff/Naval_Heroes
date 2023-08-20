using Game.Grid.Auhoring;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnerAuthoring : ScriptSpawnPoint, IEntityAuthoring
{
    [SerializeField, Range(0, 6)]
    private int m_Team = 1;

    public int team => m_Team;

    [SerializeField, OnValueChanged("OnChangeData")]
    private EntityData m_EntityData;

    private void OnChangeData()
    {
        name = m_EntityData.name;
    }

    private static readonly Color[] s_TeamsColor = new Color[] { Color.green, Color.red, Color.blue, Color.yellow, Color.black, Color.cyan, Color.magenta };

    protected Color GetColor()
    {
        return s_TeamsColor[m_Team];
    }

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var spawner = ref ecsWorld.GetPool<Spawn>().Add(entity);
        spawner.rotation = transform.rotation;
        spawner.position = transform.position;
        spawner.entityId = m_EntityData.id;
        spawner.teamId = m_Team;
    }

    public EntityData entityData => m_EntityData;

    public bool isEnable => gameObject.activeInHierarchy;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Color color = GetColor();
        //using (new Handles.DrawingScope(color, transform.localToWorldMatrix))
        //{
        //    Handles.ArrowHandleCap(0, new Vector3(0, 0, 40), Quaternion.LookRotation(Vector3.forward), 10, EventType.Repaint);
        //}

        
        if (m_EntityData != null)
        {
            var grid = m_EntityData.prefab.GetComponent<GridAuhoring>();
            if (grid != null)
            {
                using (new GizmosScope(transform.localToWorldMatrix, color))
                {
                    var rects = grid.rects;
                    Vector2 scale = grid.scale;
                    Vector2 center = grid.center;
                    for (int i = 0; i < rects.Length; i++)
                    {
                        var rect = rects[i];

                        for (int y = 0; y < rect.size.y; y++)
                        {
                            for (int x = 0; x < rect.size.x; x++)
                            {
                                Gizmos.DrawCube(new Vector3(rect.position.x * scale.x + center.x + x * scale.x, 0, rect.position.y * scale.y + center.y + y * scale.y), new Vector3(scale.x, 0, scale.y) * 0.8f);
                            }
                        }
                    }

                    //Gizmos.DrawLine(new Vector3(-10, 0, 0), new Vector3(0, 0, 10));
                    //Gizmos.DrawLine(new Vector3(10, 0, 0), new Vector3(0, 0, 10));
                    //GizmosUtility.DrawCircle(Vector3.zero, Quaternion.LookRotation(Vector3.up), new Vector2(10, 40));
                }
            }
        }
    }
#endif
}
