using Unity.Entities;
namespace Game.Merge.Events
{
    public struct MergeApplyEvent : IComponentData
    {
        public Entity a;
        public Entity b;
        public Entity result;
    }
}

