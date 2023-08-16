
using Unity.Entities;

using Game.Merge.Components;
using Game.Grid.Auhoring;
using Game.Data.Components;
using Game.Pointer.Authoring;

using UnityEngine;
using Game.Pointer.Events;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Buy Tag")]
    public class SlotBuyAuthoring : MonoBehaviour
    {
        
    }

    public class SlotBuyBaker : Baker<SlotBuyAuthoring>
    {
        public override void Bake(SlotBuyAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SlotBuyTag());
        }
    }
}