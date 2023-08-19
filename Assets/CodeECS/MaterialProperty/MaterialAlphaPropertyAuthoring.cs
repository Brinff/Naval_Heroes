using Unity.Entities;
using UnityEngine;
using Game.Rendering.Material.Components;

namespace Game.Rendering.Material.Authoring
{
    [AddComponentMenu("Game/Rendering/Material/Material Alpha Property")]
    public class MaterialAlphaPropertyAuthoring : MonoBehaviour
    {
        [Range(0,1)]
        public float alpha;
    }

    public class MaterialAlphaPropertyBaker : Baker<MaterialAlphaPropertyAuthoring>
    {
        public override void Bake(MaterialAlphaPropertyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MaterialAlphaProperty() { value = authoring.alpha });
        }
    }
}