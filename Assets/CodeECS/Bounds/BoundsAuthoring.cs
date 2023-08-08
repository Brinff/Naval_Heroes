using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BoundsAuthoring : MonoBehaviour
{
    public Bounds bounds;
    private void OnDrawGizmosSelected()
    {
        using(new GizmosScope(transform.localToWorldMatrix))
        {
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}

public class BoundsBaker : Baker<BoundsAuthoring>
{
    public override void Bake(BoundsAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new LocalBounds() { min = authoring.bounds.min, max = authoring.bounds.max });
        AddComponent(entity, new WorldBounds() { min = authoring.transform.TransformPoint(authoring.bounds.min), max = authoring.transform.TransformPoint(authoring.bounds.max) });
    }
}