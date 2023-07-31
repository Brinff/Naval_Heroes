using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OrbitViewComponent
{
    public EcsPackedEntity eyeEntity;
    public Transform transform;
    public Vector2 focalEllepse;
    public Vector2 focalCenter;
    public Vector2 orbitElllepse;
    public float height;
    public Quaternion rotation;
    public Vector3 position;
}
