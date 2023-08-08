
using Unity.Entities;

public struct MergeGrid : IComponentData
{
    public Entity gridReject;
    public Entity gridAllow;
    public Entity gridNewPosition;
    public Entity gridCurentPosition;
}
