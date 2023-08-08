
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(GridGroup))]
public partial struct GridMatrixSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (center, scale, gridMatrix) in SystemAPI.Query<RefRO<GridCenter>, RefRO<GridScale>, RefRW<GridMatrix>>())
        {
            quaternion rotation = quaternion.RotateX(math.radians(90));
            gridMatrix.ValueRW.value = float4x4.TRS(math.rotate(rotation, new float3(center.ValueRO.value, 0)), rotation, new float3(scale.ValueRO.value, 1));
        }
    }
}
