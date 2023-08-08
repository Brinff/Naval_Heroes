
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[UpdateInGroup(typeof(PointerRaycastGroup))]
[BurstCompile]
public partial struct PointerRaycastBoundsSystem : ISystemUpdate
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (ray, bufferHoveredEntity) in SystemAPI.Query<RefRO<PointerRay>, DynamicBuffer<PointerHoveredEntity>>())
        {
            foreach (var (bounds, inrercestEntity) in SystemAPI.Query<RefRO<WorldBounds>>().WithAll<PointerHandlerTag>().WithEntityAccess())
            {
                int indexEntity = -1;
                for (int i = 0; i < bufferHoveredEntity.Length; i++)
                {
                    if (bufferHoveredEntity[i].entity == inrercestEntity)
                    {
                        indexEntity = i;
                        break;
                    }
                }

                if (IntersectionRay(bounds.ValueRO.min, bounds.ValueRO.max, ray.ValueRO.origin, ray.ValueRO.direction, out float d))
                {
                    if (indexEntity < 0) bufferHoveredEntity.Add(new PointerHoveredEntity() { entity = inrercestEntity, distance = d });
                    else
                    {
                        ref var element = ref bufferHoveredEntity.ElementAt(indexEntity);
                        element.distance = d;
                    }
                }
                else
                {
                    if (indexEntity >= 0)
                    {
                        bufferHoveredEntity.RemoveAt(indexEntity);
                    }
                }
            }
        }





        //foreach (var (pressEntity, entity) in SystemAPI.Query<RefRW<PointerPressEntity>>().WithAll<PointerUpEvent>().WithEntityAccess())
        //{
        //    //Debug.Log("Press End: Raycast");
        //    if (pressEntity.ValueRO.entity != Entity.Null)
        //    {
        //        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        //        ecb.SetComponent(entity, new PointerPressEntity());
        //        Debug.Log($"Pointer End Press Entity: {state.EntityManager.GetName(pressEntity.ValueRO.entity)}");
        //    }
        //}
    }

    public static bool IntersectionRay(float3 min, float3 max, float3 origin, float3 direction, out float d)
    {
        float3 dirfrac;
        dirfrac.x = 1.0f / direction.x;
        dirfrac.y = 1.0f / direction.y;
        dirfrac.z = 1.0f / direction.z;
        // lb is the corner of AABB with minimal coordinates - left bottom, rt is maximal corner
        // r.org is origin of ray
        float t1 = (min.x - origin.x) * dirfrac.x;
        float t2 = (max.x - origin.x) * dirfrac.x;
        float t3 = (min.y - origin.y) * dirfrac.y;
        float t4 = (max.y - origin.y) * dirfrac.y;
        float t5 = (min.z - origin.z) * dirfrac.z;
        float t6 = (max.z - origin.z) * dirfrac.z;

        float tmin = math.max(math.max(math.min(t1, t2), math.min(t3, t4)), math.min(t5, t6));
        float tmax = math.min(math.min(math.max(t1, t2), math.max(t3, t4)), math.max(t5, t6));

        // if tmax < 0, ray (line) is intersecting AABB, but the whole AABB is behind us
        if (tmax < 0)
        {
            d = tmax;
            return false;
        }

        // if tmin > tmax, ray doesn't intersect AABB
        if (tmin > tmax)
        {
            d = tmax;
            return false;
        }
        d = tmin;
        return true;
    }
}
