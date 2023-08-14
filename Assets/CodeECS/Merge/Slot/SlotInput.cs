using Unity.Entities;
using Unity.Mathematics;

namespace Game.Merge.Components
{
    public struct SlotInputData : IComponentData
    {
        public Entity itemEntity;
        public float3 position;
    }
}

