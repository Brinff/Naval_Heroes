using Unity.Entities;
using Unity.Mathematics;

namespace Game.Merge.Components
{
    public struct SlotOutputData : IComponentData
    {
        public Entity itemEntity;
        public bool isPosible;
        public float3 position;
        public float3 targetPosition;
    }
}

