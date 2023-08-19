
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Game.Rendering.Material.Components
{
    [MaterialProperty("_Color")]
    public struct MaterialColorProperty : IComponentData
    {
        public float4 value;
    }
}
