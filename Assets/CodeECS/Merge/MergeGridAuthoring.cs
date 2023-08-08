using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MergeGridAuthoring : MonoBehaviour
{

}

public class MergeGridBaker : Baker<MergeGridAuthoring>
{
    public override void Bake(MergeGridAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        var mergeGrid = new MergeGrid();
        AddComponent(entity, mergeGrid);
    }
}
