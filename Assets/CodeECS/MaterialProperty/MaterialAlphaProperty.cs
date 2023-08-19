using Unity.Entities;
using Unity.Rendering;

namespace Game.Rendering.Material.Components
{
    [MaterialProperty("_Alpha")]
    public struct MaterialAlphaProperty : IComponentData
    {
        public float value;
    }
}

