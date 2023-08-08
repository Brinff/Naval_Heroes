using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct GridRenderer : ISharedComponentData, IQueryTypeParameter, IEquatable<GridRenderer>
{
    [SerializeField]
    public Material material;

    public bool Equals(GridRenderer other)
    {
        return GetHashCode().Equals(other.GetHashCode());
    }

    public override int GetHashCode()
    {
        return material?.GetHashCode() ?? 0;
    }
}
