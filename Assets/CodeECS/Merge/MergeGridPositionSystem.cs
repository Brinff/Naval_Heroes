

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(MergeGroup))]

public partial struct MergeGridPositionSystem : ISystemUpdate
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);


        foreach (var (mergeDragPointer, mergeEntity) in SystemAPI.Query<RefRO<MergeDragPointer>>().WithAll<MergeBeginDragEvent, GridParent>().WithEntityAccess())
        {
            if (mergeDragPointer.ValueRO.pointer == Entity.Null) continue;
            SystemAPI.SetComponentEnabled<GridParent>(mergeEntity, false);
        }

        foreach (var (mergeGrid, mergeGridTransform, mergeGridEntity) in SystemAPI.Query<RefRO<MergeGrid>, GridTransformAspect>().WithEntityAccess())
        {
            var gridNewPositionRects = SystemAPI.GetBuffer<GridRect>(mergeGrid.ValueRO.gridNewPosition);
            gridNewPositionRects.Clear();

            foreach (var (mergeDragPointer, dragGridTrnasform, dragGridEntity) in SystemAPI.Query<RefRO<MergeDragPointer>, GridTransformAspect>().WithEntityAccess())
            {
                if (mergeDragPointer.ValueRO.pointer == Entity.Null) continue;

                var ray = SystemAPI.GetComponent<PointerRay>(mergeDragPointer.ValueRO.pointer);
                if (mergeGridTransform.Raycast(ray.origin, ray.direction, out float3 point))
                {
                    //dragGridTrnasform.position = point;

                    var dragGridRects = dragGridTrnasform.GetProjectRects(point, mergeGridTransform.GetGridWorldToLocal());
                    var mergeGridRects = mergeGridTransform.GetRects();
                    bool isAnyOverlap = false;
                    float2 position = float2.zero;
                    for (int i = 0; i < dragGridRects.Length; i++)
                    {
                        for (int j = 0; j < mergeGridRects.Length; j++)
                        {
                            if (GridUtility.IsOverlap(dragGridRects[i], mergeGridRects[j]))
                            {
                                var projectRect = dragGridRects[i].GetClampRect(mergeGridRects[j]).GetRoundedRect();
                                position = projectRect.position;
                                isAnyOverlap = true;
                                gridNewPositionRects.Add(projectRect);
                            }
                        }    
                    }

                    if (isAnyOverlap)
                    {
                        if (SystemAPI.HasComponent<GridTarget>(dragGridEntity))
                        {
                            var gridTaret = SystemAPI.GetComponentRW<GridTarget>(dragGridEntity);
                            gridTaret.ValueRW.grid = mergeGridEntity;
                            gridTaret.ValueRW.position = position;
                        }
                        else beginEcb.AddComponent<GridTarget>(dragGridEntity, new GridTarget() { grid = mergeGridEntity, position = position });

                    }
                    else
                    {
                        if (SystemAPI.HasComponent<GridTarget>(dragGridEntity)) endEcb.RemoveComponent<GridTarget>(dragGridEntity);
                    }
                }
                else
                {
                    if (SystemAPI.HasComponent<GridTarget>(dragGridEntity)) endEcb.RemoveComponent<GridTarget>(dragGridEntity);
                }
            }
        }

        foreach (var (mergeDragPointer, mergeEntity) in SystemAPI.Query<RefRO<MergeDragPointer>>().WithAll<GridTarget, MergeEndDragEvent>().WithEntityAccess())
        {
            if (mergeDragPointer.ValueRO.pointer == Entity.Null) continue;
            if (SystemAPI.HasComponent<GridTarget>(mergeEntity)) endEcb.RemoveComponent<GridTarget>(mergeEntity);
        }

        //foreach (var (mergeDragPointer, mergeEntity) in SystemAPI.Query<RefRO<MergeDragPointer>>().WithAll<MergeEndDragEvent>().WithDisabled<GridParent>().WithEntityAccess())
        //{
        //    if (mergeDragPointer.ValueRO.pointer == Entity.Null) break;
        //    if (SystemAPI.HasComponent<GridTarget>(mergeEntity)) endEcb.RemoveComponent<GridTarget>(mergeEntity);
        //    endEcb.SetComponentEnabled<GridParent>(mergeEntity, true);
        //}
    }
}
