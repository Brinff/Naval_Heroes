using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewType { None, Orbit, Zoom, Home }
public struct LookAtViewComponent
{
    public ViewType activeView;
    public EcsPackedEntity eyeEntity;
    public Transform transform;
    public Vector3 position;
    public Quaternion rotation;
    public float distance;

    public float sensitivityHorizontal;
    public float sensitivityVertical;
    public float sencitivityScale;
    public float tiltAngleMin;
    public float tiltAngleMax;
}
