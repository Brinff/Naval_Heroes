using Game.Merge.Components;
using Unity.Entities;
using UnityEngine;
namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot")]
    public class SlotAuthoring : MonoBehaviour
    {

    }

    public class SlotBaker : Baker<SlotAuthoring>
    {
        public override void Bake(SlotAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<Slot>(entity);
            AddComponent<SlotInputData>(entity);
            AddComponent<SlotOutputData>(entity);
        }
    }
}