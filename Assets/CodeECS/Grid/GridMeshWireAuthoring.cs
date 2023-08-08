using UnityEngine;
using Unity.Entities;

public class GridMeshWireAuthoring : MonoBehaviour
{
    public float widht = 0.1f;
    public float tails = 0;
}

public class GridMeshWireBaker : Baker<GridMeshWireAuthoring>
{
    public override void Bake(GridMeshWireAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Renderable);
        AddComponent(entity, new GridMeshWire() { width = authoring.widht, tails = authoring.tails });
    }
}
