using Unity.Entities;
using UnityEngine;

using Game.Merge.Components;
using Game.Grid.Auhoring;
using Game.Pointer.Authoring;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Grid Tag")]
    public class SlotGridTagAuthoring : MonoBehaviour
    {

    }

    public class SlotGridTagBaker : Baker<SlotGridTagAuthoring>
    {
        public override void Bake(SlotGridTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SlotGridTag());
        }
    }
}