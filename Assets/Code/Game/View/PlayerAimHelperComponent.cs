using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerAimHelperComponent
{
    public Vector3 point;
    public EcsPackedEntity target;
    public Quaternion deltaRotationY;
    public Quaternion deltaRotationX;
    public Quaternion offsetRotationY;
    public Quaternion offsetRotationX;
}
