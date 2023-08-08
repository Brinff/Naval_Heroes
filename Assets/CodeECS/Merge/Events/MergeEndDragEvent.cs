
using Unity.Entities;

public struct MergeEndDragEvent : IComponentData, IEnableableComponent
{
    public Entity pointer;
}
