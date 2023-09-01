using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public struct MovePath
{
    public Spline spline;
    public float position;
    public Vector3 target;
    public float targetTime;
    public float damage;
    public bool isFire;
}
