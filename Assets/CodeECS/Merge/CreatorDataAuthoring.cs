using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CreatorDataAuthoring : MonoBehaviour
{
    public GameObject prefab;
}

public class CreatorDataBaker : Baker<CreatorDataAuthoring>
{
    public override void Bake(CreatorDataAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new CreatorData() { prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic) });
    }
}
