using Game.Grid.Auhoring;
using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuoyancyAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private float m_WaterLine;
    [SerializeField, Range(0, 1)]
    private float m_Slope = 1;
    public bool isEnable => gameObject.activeInHierarchy;

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var shipBuoyancy = ref ecsWorld.GetPool<ShipBuoyancy>().Add(entity);
        shipBuoyancy.waterLine = m_WaterLine;
        shipBuoyancy.slope = m_Slope;
    }

    private void OnDrawGizmos()
    {
        using (new GizmosScope(transform.localToWorldMatrix, new Color(0, 0, 1f, 0.5f)))
        {
            GridAuhoring gridAuhoring = GetComponent<GridAuhoring>();
            if (gridAuhoring != null)
            {
                Bounds localBounds = new Bounds(gridAuhoring.center, Vector3.zero);

                for (int i = 0; i < gridAuhoring.rects.Length; i++)
                {
                    var rect = gridAuhoring.rects[i];

                    for (int y = 0; y < rect.size.y; y++)
                    {
                        for (int x = 0; x < rect.size.x; x++)
                        {
                            Bounds bounds = new Bounds(new Vector3(rect.position.x * gridAuhoring.scale.x + gridAuhoring.center.x + x * gridAuhoring.scale.x, 0, rect.position.y * gridAuhoring.scale.y + gridAuhoring.center.y + y * gridAuhoring.scale.y), new Vector3(gridAuhoring.scale.x, 0.1f, gridAuhoring.scale.y));
                            localBounds.Encapsulate(bounds);
                        }
                    }


                }
                Vector3 center = localBounds.center;
                center.y = m_WaterLine;
                Vector3 size = localBounds.size;
                size.y = 0.1f;
                Gizmos.DrawCube(center, size);
            }

        }
    }
}
