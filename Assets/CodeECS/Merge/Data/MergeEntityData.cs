using Unity.Entities;
namespace Game.Merge.Components
{
    public struct MergeEntityData : IBufferElementData
    {
        public int a;
        public int b;
        public int result;
    }
}

