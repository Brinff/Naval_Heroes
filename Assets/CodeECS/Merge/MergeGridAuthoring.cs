using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MergeGridAuthoring : MonoBehaviour
{
    public GameObject gridNewPosition;
    public GameObject gridCurrentPosition;
    public GameObject gridReject;
    public GameObject gridAllow;
}

public class MergeGridBaker : Baker<MergeGridAuthoring>
{
    public override void Bake(MergeGridAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        var mergeGrid = new MergeGrid();
        mergeGrid.gridCurentPosition = GetEntity(authoring.gridCurrentPosition, TransformUsageFlags.None);
        mergeGrid.gridNewPosition = GetEntity(authoring.gridNewPosition, TransformUsageFlags.None);
        mergeGrid.gridReject = GetEntity(authoring.gridReject, TransformUsageFlags.None);
        mergeGrid.gridAllow = GetEntity(authoring.gridAllow, TransformUsageFlags.None);
        AddComponent(entity, mergeGrid);
    }
}
