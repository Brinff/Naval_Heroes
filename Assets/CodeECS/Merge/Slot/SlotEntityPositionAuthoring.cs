using Unity.Entities;
using UnityEngine;

using Game.Merge.Components;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Entity Position")]
    public class SlotEntityPositionAuthoring : MonoBehaviour
    {
        public Vector3 position;
    }

    public class SlotEntityPositionAuthoringBaker : Baker<SlotEntityPositionAuthoring>
    {
        public override void Bake(SlotEntityPositionAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SlotEntityPosition() { value = authoring.position });
        }
    }
}