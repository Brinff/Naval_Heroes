using Game.Eye.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class CameraLinkSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var (cameraLink, localToWorld) in SystemAPI.Query<CameraLink, LocalToWorld>())
        {
            cameraLink.value.transform.position = localToWorld.Position;
            cameraLink.value.transform.rotation = localToWorld.Rotation;
        }
    }
}

