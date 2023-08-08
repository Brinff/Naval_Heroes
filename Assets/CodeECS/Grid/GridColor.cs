using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[MaterialProperty("_Color")]
public struct GridColor : IComponentData
{
    [ColorUsage(true, true)]
    public float4 value;
}
