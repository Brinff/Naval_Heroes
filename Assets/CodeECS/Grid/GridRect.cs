using Unity.Entities;
using Unity.Mathematics;

public struct GridRect : IBufferElementData
{
    public float2 position;
    public int2 size;
}

