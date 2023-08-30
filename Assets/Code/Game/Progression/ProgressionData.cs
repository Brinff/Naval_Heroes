using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Progression")]
public class ProgressionData : ScriptableObject
{
    public float value;
    public float pow;
    public float factor;
    public int maxStep = 100;

    [NonSerialized]
    public AnimationCurve curve;

    public float GetResult(float position)
    {
        return value + Mathf.Pow(position, pow) * factor;
    }
}
