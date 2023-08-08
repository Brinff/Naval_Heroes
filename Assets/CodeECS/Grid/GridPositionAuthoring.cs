using Unity.Entities;
using UnityEngine;

public class GridPositionAuthoring : MonoBehaviour
{
    public Vector2Int position;
}

public class GridPositionAuthoringBaker : Baker<GridPositionAuthoring>
{
    public override void Bake(GridPositionAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new GridPosition() { value = (Vector2)authoring.position });
    }
}