using Unity.Entities;
namespace Game.Merge.Components
{
    public struct SlotMergeOutputData : IComponentData
    {
        public Entity itemEntityA;
        public Entity itemEntityB;
        public int result;
    }
}

