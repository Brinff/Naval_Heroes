
using Unity.Entities;
using Unity.Mathematics;

namespace Game.Merge.Components
{
    [WriteGroup(typeof(SlotEntityPosition))]
    public struct SlotEntityGridPosition : IComponentData
    {
        public float3 value;
    }

    public struct SlotEntityPosition : IComponentData
    {
        public float3 value;
    }
}
