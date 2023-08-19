using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

using Game.Rendering.Material.Components;

namespace Game.Grid.Auhoring
{
    [RequireComponent(typeof(GridAuhoring))]
    //[AddComponentMenu("Game/Grid/Grid ")]
    public class GridCompositionAuthoring : MonoBehaviour
    {
        public GameObject gridField;
        public GameObject gridNewPosition;
        public GameObject gridCurrentPosition;
        public GameObject gridReject;
        public GameObject gridAllow;
    }

    public class GridCompositionBaker : Baker<GridCompositionAuthoring>
    {
        public override void Bake(GridCompositionAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            GridComposition mergeGridRenderer = new GridComposition();

            mergeGridRenderer.field = GetEntity(authoring.gridField, TransformUsageFlags.None);          
            mergeGridRenderer.newPosition = GetEntity(authoring.gridNewPosition, TransformUsageFlags.None);         
            mergeGridRenderer.curentPosition = GetEntity(authoring.gridCurrentPosition, TransformUsageFlags.None);         
            mergeGridRenderer.reject = GetEntity(authoring.gridReject, TransformUsageFlags.None);         
            mergeGridRenderer.allow = GetEntity(authoring.gridAllow, TransformUsageFlags.None);
            
            AddComponent(entity, mergeGridRenderer);
        }

        //public Entity AddGridRenderer(Entity parent, Material material, float width, float tails, bool isAddRects)
        //{
        //    if (material == null) return Entity.Null;
        //    var entity = CreateAdditionalEntity(TransformUsageFlags.Renderable, false, material.name);

        //    AddComponent(entity, new Parent() { Value = parent });
        //    AddComponent(entity, new LocalTransform() { Position = float3.zero, Rotation = quaternion.identity, Scale = 1 });

        //    GridBaker.AddComponents(GetComponent<GridAuhoring>(), entity, this, isAddRects);

        //    AddComponent(entity, new GridMeshWire() { width = width, tails = tails });
        //    AddSharedComponentManaged(entity, new GridRenderer() { material = material });

        //    AddComponent(entity, new MaterialAlphaProperty() { value = 1 });

        //    return entity;
        //}
    }
}