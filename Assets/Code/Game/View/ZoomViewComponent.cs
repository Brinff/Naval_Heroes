using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ZoomViewComponent
{
    public EcsPackedEntity eyeEntity;
    public Transform transform;
    public float height;
    public float zoomFactor;
    public float fieldOfView;
    public Quaternion rotation;
    public Vector3 position;
}
