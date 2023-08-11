using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#if UNITY_EDITOR

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct BoundsGizmosSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var bounds in SystemAPI.Query<RefRO<WorldBounds>>())
        {
            var min = bounds.ValueRO.min;
            var max = bounds.ValueRO.max;

            Debug.DrawLine(min, new Vector3(min.x, min.y, max.z));
            Debug.DrawLine(min, new Vector3(min.x, max.y, min.z));
            Debug.DrawLine(min, new Vector3(max.x, min.y, min.z));

            Debug.DrawLine(max, new Vector3(max.x, max.y, min.z));
            Debug.DrawLine(max, new Vector3(max.x, min.y, max.z));
            Debug.DrawLine(max, new Vector3(min.x, max.y, max.z));
        }
    }
}

#endif
