
using Unity.Entities;
using UnityEngine;

namespace Game.Grid.Auhoring
{
    [AddComponentMenu("Game/Grid/Grid Renderer")]
    public class GridRendererAuthoring : MonoBehaviour
    {
        public Material material;
    }

    public class GridRendererBaker : Baker<GridRendererAuthoring>
    {
        public override void Bake(GridRendererAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Renderable);
            AddSharedComponentManaged(entity, new GridRenderer() { material = authoring.material });
        }
    }
}
