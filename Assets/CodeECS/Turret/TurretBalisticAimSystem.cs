using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct TurretBalisticAimSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        ComponentLookup<RotationConstrainTargetComponent> rotationConstrainTargetLookup = state.GetComponentLookup<RotationConstrainTargetComponent>();
        ComponentLookup<LocalToWorld> localToWorldLookup = state.GetComponentLookup<LocalToWorld>();
        foreach (var (turretAimPoint, turretBalisticAim, turretConstains) in SystemAPI.Query<RefRO<TurretAimPoint>, RefRO<TurretBalisticAim>, DynamicBuffer<TurretConstrains>>())
        {
            float3 direction = GetDirection(localToWorldLookup.GetRefRO(turretBalisticAim.ValueRO.origin).ValueRO.Position, turretAimPoint.ValueRO.value, turretBalisticAim.ValueRO.velocity);
            quaternion rotation = quaternion.LookRotationSafe(direction, new float3(0, 1, 0));
            foreach (var item in turretConstains)
            {
                rotationConstrainTargetLookup.GetRefRW(item.value).ValueRW.rotation = rotation;
            }
        }
        
    }

    public static float3 GetDirection(float3 start, float3 end, float velocity)
    {
        float3 dir = end - start;

        float vSqr = velocity * velocity;
        float y = dir.y;

        float x = math.lengthsq(dir);
        float g = 9.81f;

        float uRoot = vSqr * vSqr - g * (g * (x) + (2.0f * y * vSqr));

        if (uRoot < 0.0f)
        {
            return math.normalize(dir);
        }

        float xE = g * math.sqrt(x);
        float yE = vSqr + math.sqrt(uRoot);
        float3 elevation = math.normalize(dir) * yE + new float3(0, 1, 0) * xE;

        return math.normalize(elevation); //-Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(uRoot));
    }
}
