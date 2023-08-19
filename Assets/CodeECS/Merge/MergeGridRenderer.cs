using Unity.Entities;

public struct GridComposition : IComponentData
{
    public Entity field;
    public Entity reject;
    public Entity allow;
    public Entity newPosition;
    public Entity curentPosition;
}

