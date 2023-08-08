
using Unity.Entities;


public struct CreatorData : IComponentData
{
    public Entity instance;
    public Entity prefab;
}
