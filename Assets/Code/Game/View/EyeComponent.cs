using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct EyeComponent
{
    public Camera camera;
    public Transform transform;
    public Vector3 position;
    public Quaternion rotation;
    public float fieldOfView;
    public EcsPackedEntity view;
}
