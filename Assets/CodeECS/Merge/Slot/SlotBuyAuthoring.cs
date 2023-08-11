
using Unity.Entities;

using Game.Merge.Components;
using Game.Grid.Auhoring;
using Game.Data.Components;
using Game.Pointer.Authoring;

using UnityEngine;


namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Buy")]
    public class SlotBuyAuthoring : MonoBehaviour
    {
        
    }

    public class SlotBuyBaker : Baker<SlotBuyAuthoring>
    {
        public override void Bake(SlotBuyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            PointerHandlerBaker.Bake(this, entity, PointerHandlerEvent.BeginDrag | PointerHandlerEvent.UpdateDrag | PointerHandlerEvent.EndDrag | PointerHandlerEvent.Drop | PointerHandlerEvent.Click);
            var gridRectAuthorin = authoring.GetComponent<GridAuhoring>();
            AddComponent(entity, gridRectAuthorin.GetBounds(5));
            AddComponent(entity, new SlotBuy());
            AddComponent(entity, new SlotEntity());
        }
    }
}