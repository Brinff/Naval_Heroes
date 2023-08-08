using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequireComponent(typeof(GridAuhoring))]
public class MergeGridRendererAuthoring : MonoBehaviour
{
    public float width = 1;
    public float tails = 30f;
    public Material fieldGridMaterial;
    public Material newPositionGridMaterial;
    public Material currentPositionGridMaterial;
    public Material rejectGridMaterial;
    public Material allowGridMaterial;
}

public class MergeGridRendererAuthoringBaker : Baker<MergeGridRendererAuthoring>
{
    public override void Bake(MergeGridRendererAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        MergeGridRenderer mergeGridRenderer = new MergeGridRenderer();
        mergeGridRenderer.field = AddGridRenderer(entity, authoring.fieldGridMaterial, authoring.width, authoring.tails, true);
        mergeGridRenderer.newPosition = AddGridRenderer(entity, authoring.newPositionGridMaterial, authoring.width, 0, false);
        mergeGridRenderer.curentPosition = AddGridRenderer(entity, authoring.currentPositionGridMaterial, authoring.width, 0, false);
        mergeGridRenderer.reject = AddGridRenderer(entity, authoring.rejectGridMaterial, authoring.width, 0, false);
        mergeGridRenderer.allow = AddGridRenderer(entity, authoring.allowGridMaterial, authoring.width, 0, false);
        AddComponent(entity, mergeGridRenderer);
    }

    public Entity AddGridRenderer(Entity parent, Material material, float width, float tails, bool isAddRects)
    {
        if (material == null) return Entity.Null;
        var entity = CreateAdditionalEntity(TransformUsageFlags.Renderable, false, material.name);

        AddComponent(entity, new Parent() { Value = parent });
        AddComponent(entity, new LocalTransform() { Position = float3.zero, Rotation = quaternion.identity, Scale = 1 });

        GridBaker.AddComponents(GetComponent<GridAuhoring>(), entity, this, isAddRects);

        AddComponent(entity, new GridMeshWire() { width = width, tails = tails });
        AddSharedComponentManaged(entity, new GridRenderer() { material = material });

        return entity;
    }
}