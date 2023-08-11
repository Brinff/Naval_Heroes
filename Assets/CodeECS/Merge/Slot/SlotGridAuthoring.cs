using Unity.Entities;
using UnityEngine;

using Game.Merge.Components;
using Game.Grid.Auhoring;
using Game.Pointer.Authoring;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Grid")]
    [RequireComponent(typeof(GridAuhoring))]
    public class SlotGridAuthoring : MonoBehaviour
    {

    }

    public class SlotGridBaker : Baker<SlotGridAuthoring>
    {
        public override void Bake(SlotGridAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SlotEntity());
            AddComponent(entity, new SlotPosition());
            PointerHandlerBaker.Bake(this, entity, PointerHandlerEvent.BeginDrag | PointerHandlerEvent.UpdateDrag | PointerHandlerEvent.EndDrag | PointerHandlerEvent.Drop | PointerHandlerEvent.Enter | PointerHandlerEvent.Exit);
            var gridRectAuthorin = authoring.GetComponent<GridAuhoring>();
            AddComponent(entity, gridRectAuthorin.GetBounds(5));
            AddComponent(entity, new SlotGrid());
        }
    }
}