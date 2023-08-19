using Unity.Entities;
using UnityEngine;
using Game.Rendering.Material.Components;

namespace Game.Rendering.Material.Authoring
{
    [AddComponentMenu("Game/Rendering/Material/Material Color Property")]
    public class MaterialColorPropertyAuthoring : MonoBehaviour
    {
        [ColorUsage(true, true)]
        public Color color;
    }

    public class MaterialColorPropertyBaker : Baker<MaterialColorPropertyAuthoring>
    {
        public override void Bake(MaterialColorPropertyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MaterialColorProperty() { value = (Vector4)authoring.color });
        }
    }
}