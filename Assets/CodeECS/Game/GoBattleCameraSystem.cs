using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Game.Eye.Components;
using Game.Mission.Events;
using Game.Battle.Events;

[BurstCompile]
public partial struct GoBattleCameraSystem : ISystem
{
    private EntityQuery cameraMainQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        cameraMainQuery = SystemAPI.QueryBuilder().WithAll<MainCameraTag>().Build();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var cameraMainEntity = cameraMainQuery.GetSingletonEntity();
        foreach (var (localToWorld, entity) in SystemAPI.Query<RefRO<LocalToWorld>>().WithAll<GoBattleEvent, BattleCameraTag>().WithEntityAccess())
        {
            var localTransform = SystemAPI.GetComponentRW<LocalTransform>(cameraMainEntity); 
            localTransform.ValueRW.Position = localToWorld.ValueRO.Position;
            localTransform.ValueRW.Rotation = localToWorld.ValueRO.Rotation;
        }
    }
}

