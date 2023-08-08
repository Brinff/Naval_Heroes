using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct MergeGridParentSystem : ISystemUpdate
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (mergeGrid, mergeGridTransform, mergeGridEntity) in SystemAPI.Query<RefRO<MergeGrid>, GridTransformAspect>().WithEntityAccess())
        {
            //var currentRects = SystemAPI.GetBuffer<GridRect>(mergeGrid.ValueRO.gridCurentPosition);
            //currentRects.Clear();

            float4x4 projectMatrix = mergeGridTransform.GetGridWorldToLocal();

            foreach (var (gridPosition, gridTransform, gridEntity) in SystemAPI.Query<RefRO<GridPosition>, GridTransformAspect>().WithEntityAccess())
            {
                if (SystemAPI.HasComponent<GridParent>(gridEntity))
                {
                    var gridParent = SystemAPI.GetComponentRW<GridParent>(gridEntity);
                    if (gridParent.ValueRO.value != mergeGridEntity) continue;

                    var position = math.transform(mergeGridTransform.GetGridLocalToWorld(), new float3(gridPosition.ValueRO.value, 0));

                    if (SystemAPI.IsComponentEnabled<GridParent>(gridEntity)) gridTransform.position = position + gridTransform.GetOffset();

                    var projectRects = gridTransform.GetProjectRects(gridTransform.position, projectMatrix);

                    for (int i = 0; i < projectRects.Length; i++)
                    {
                        //currentRects.Add(projectRects[i]);
                    }
                }
            }
        }
    }
}

