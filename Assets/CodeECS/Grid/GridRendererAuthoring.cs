using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class GridRendererAuthoring : MonoBehaviour
{
    public Material material;
}

public class GridRendererBaker : Baker<GridRendererAuthoring>
{
    public override void Bake(GridRendererAuthoring authoring)
    {
        var entity = GetEntity(authoring, TransformUsageFlags.Renderable);
        AddSharedComponentManaged(entity, new GridRenderer() { material = authoring.material });
    }
}
