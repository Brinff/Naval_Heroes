using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MergeGridPlacerAuhoring : MonoBehaviour
{

}

public class MergeGridPlacerBaker : Baker<MergeGridPlacerAuhoring>
{
    public override void Bake(MergeGridPlacerAuhoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new MergeDragPointer());
        //AddComponent(entity, new GridTarget());
        //AddComponent(entity, new GridPosition());

        AddComponent<MergeBeginDragEvent>(entity);
        SetComponentEnabled<MergeBeginDragEvent>(entity, false);

        AddComponent<MergeUpdateDragEvent>(entity);
        SetComponentEnabled<MergeUpdateDragEvent>(entity, false);

        AddComponent<MergeEndDragEvent>(entity);
        SetComponentEnabled<MergeEndDragEvent>(entity, false);
    }
}