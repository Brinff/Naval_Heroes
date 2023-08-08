using Unity.Entities;
using Unity.Mathematics;

public struct GridTarget : IComponentData
{
    public Entity grid;
    public float2 position;
}

