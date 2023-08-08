using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(MergeGroup), OrderLast = true)]
[BurstCompile]
public partial struct MergeGridApplySystem : ISystemUpdate
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (applyGridTransform, applyGridTarget, applyGridEntity) in SystemAPI.Query<GridTransformAspect, RefRO<GridTarget>>().WithAll<MergeEndDragEvent>().WithEntityAccess())
        {
            var targetRectTransform = SystemAPI.GetAspect<GridTransformAspect>(applyGridTarget.ValueRO.grid);
            var newPosition = targetRectTransform.GetGridLocalToWorld().TransformPoint(new float3(applyGridTarget.ValueRO.position, 0));
            applyGridTransform.position = newPosition + applyGridTransform.GetOffset();

            if (SystemAPI.HasComponent<GridPosition>(applyGridEntity)) beginEcb.SetComponent(applyGridEntity, new GridPosition() { value = applyGridTarget.ValueRO.position });
            else beginEcb.AddComponent(applyGridEntity, new GridPosition() { value = applyGridTarget.ValueRO.position });

            if (SystemAPI.HasComponent<GridParent>(applyGridEntity)) beginEcb.SetComponent(applyGridEntity, new GridParent() { value = applyGridTarget.ValueRO.grid });
            else beginEcb.AddComponent(applyGridEntity, new GridParent() { value = applyGridTarget.ValueRO.grid });
               
            beginEcb.SetComponentEnabled<GridParent>(applyGridEntity, true);
        }
    }
}

