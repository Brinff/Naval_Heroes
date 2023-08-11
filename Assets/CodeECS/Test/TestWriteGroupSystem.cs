using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct TestWriteGroupSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (a,b) in SystemAPI.Query<RefRW<TestA>, RefRW<TestB>>().WithOptions(EntityQueryOptions.FilterWriteGroup))
        {
            b.ValueRW.value++;
        }
    }
}

