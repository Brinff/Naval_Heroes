
using Unity.Entities;
using Unity.Transforms;
public partial struct CreatorSystem : ISystemUpdate
{
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (creator, localToWorld, entity) in SystemAPI.Query<RefRW<CreatorData>, RefRO<LocalToWorld>>().WithEntityAccess())
        {
            if (creator.ValueRO.instance == Entity.Null)
            {
                var instanceEntity = beginEcb.Instantiate(creator.ValueRO.prefab);
                creator.ValueRW.instance = instanceEntity;
                beginEcb.SetComponent<LocalTransform>(instanceEntity, new LocalTransform() { Position = localToWorld.ValueRO.Position, Rotation = localToWorld.ValueRO.Rotation, Scale = 1 });
                beginEcb.SetComponent(entity, creator.ValueRO);
            }
        }

        //foreach (var (pointerEvent, creatorData, entity) in SystemAPI.Query<RefRO<PointerDownEvent>, RefRO<CreatorData>>().WithEntityAccess())
        //{
        //    //Debug.Log($"Down on entity: {state.EntityManager.GetName(entity)}");
        //    foreach (var id in SystemAPI.Query<RefRO<PointerId>>())
        //    {
        //        if (pointerEvent.ValueRO.value.HasFlag(id.ValueRO.value))
        //        {
        //            var entityGridPlace = beginEcb.CreateEntity();
        //            beginEcb.AddComponent(entityGridPlace, new CreatorGridData() { id = id.ValueRO.value, instance = creatorData.ValueRO.instance });
        //            //Debug.Log($"Down point: {id.ValueRO.value} {state.EntityManager.GetName(entity)}");
        //        }
        //    }
        //}

        //foreach (var creatorGridData in SystemAPI.Query<RefRO<CreatorGridData>>())
        //{
        //    foreach (var (id, ray) in SystemAPI.Query<RefRO<PointerId>, RefRO<PointerRay>>())
        //    {
        //        if (id.ValueRO.value == creatorGridData.ValueRO.id)
        //        {
        //            if (RaycastPlane(new float3(0, 1, 0), 0, ray.ValueRO.origin, ray.ValueRO.direction, out float distance))
        //            {
        //                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(creatorGridData.ValueRO.instance);
        //                localTransform.ValueRW.Position = ray.ValueRO.origin + ray.ValueRO.direction * distance;
        //            }
        //        }
        //    }
        //}

        //foreach (var (pointerEvent, localToWorld, entity) in SystemAPI.Query<RefRO<PointerUpEvent>, RefRO<LocalToWorld>>().WithAll<CreatorData>().WithEntityAccess())
        //{
        //    //Debug.Log($"Up on entity: {state.EntityManager.GetName(entity)}");
        //    foreach (var (creatorGridData, creatorEntity) in SystemAPI.Query<RefRO<CreatorGridData>>().WithEntityAccess())
        //    {
        //        foreach (var id in SystemAPI.Query<RefRO<PointerId>>())
        //        {
        //            if (id.ValueRO.value == creatorGridData.ValueRO.id)
        //            {
        //                RefRW<LocalTransform> localTransform = SystemAPI.GetComponentRW<LocalTransform>(creatorGridData.ValueRO.instance);
        //                localTransform.ValueRW.Position = localToWorld.ValueRO.Position;
        //                endEcb.DestroyEntity(creatorEntity);
        //            }
        //            //if (pointerEvent.ValueRO.value.HasFlag(id.ValueRO.value))
        //            //{
        //            //    //Debug.Log($"Up point: {id.ValueRO.value} {state.EntityManager.GetName(entity)}");
        //            //}
        //        }
        //    }
        //}
    }


}
