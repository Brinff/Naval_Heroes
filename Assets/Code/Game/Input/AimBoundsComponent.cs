using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AimBoundsComponent
{
    public Transform transform;

    public Bounds bounds;

    public Bounds enterDirectionBounds;
    public Bounds enterFaceBounds;
    public float enterBoundsExpand;

    public Bounds exitBounds;
    public float exitBoundsExpand;

    public Vector3 center;
    public Vector3 position;
    public Vector3 velocity;
}
