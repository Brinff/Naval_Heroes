using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;


public static class Geometry
{
    public static bool OverlapAABB(float3 aMin, float3 aMax, float3 bMin, float3 bMax)
    {
        return math.all((aMax >= bMin) & (aMin <= bMax)); 
    }
}

