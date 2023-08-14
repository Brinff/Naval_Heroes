using Unity.Entities;
using Unity.Mathematics;

namespace Game.Merge.Events
{
    public struct SlotAddItemEvent : IComponentData
    {
        public Entity itemEntity;
        public float3 position;
    }
}

