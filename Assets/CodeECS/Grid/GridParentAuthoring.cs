using Unity.Entities;
using UnityEngine;

public class GridParentAuthoring : MonoBehaviour
{
    public GameObject parent;
}

public class GridParentAuthoringBaker : Baker<GridParentAuthoring>
{
    public override void Bake(GridParentAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new GridParent() { value = GetEntity(authoring.parent, TransformUsageFlags.None) });
    }
}