using Unity.Entities;

using Game.Grid.Auhoring;
using Game.Merge.Components;
using Game.Pointer.Authoring;

using UnityEngine;


namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Merge")]
    public class SlotMergeAuthoring : MonoBehaviour
    {

    }

    public class SlotMergeBaker : Baker<SlotMergeAuthoring>
    {
        public override void Bake(SlotMergeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            PointerHandlerBaker.Bake(this, entity, PointerHandlerEvent.BeginDrag | PointerHandlerEvent.UpdateDrag | PointerHandlerEvent.EndDrag | PointerHandlerEvent.Drop);
            var gridRectAuthorin = authoring.GetComponent<GridAuhoring>();
            AddComponent(entity, gridRectAuthorin.GetBounds(5));
            AddComponent(entity, new SlotMerge());
            AddComponent(entity, new SlotEntity());
        }
    }
}