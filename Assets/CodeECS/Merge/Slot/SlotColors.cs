using Unity.Entities;
using Unity.Mathematics;

namespace Game.Merge.Components
{
    public struct SlotColors : IComponentData
    {
        public float4 allow;
        public float4 reject;
        public float4 current;
    }
}

