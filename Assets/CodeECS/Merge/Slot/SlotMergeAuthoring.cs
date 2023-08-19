using Unity.Entities;

using Game.Grid.Auhoring;
using Game.Merge.Components;
using Game.Pointer.Authoring;

using UnityEngine;
using Game.Rendering.Material.Components;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Slot Merge")]
    public class SlotMergeAuthoring : MonoBehaviour
    {
        [ColorUsage(true, true)]
        public Color current;
        [ColorUsage(true, true)]
        public Color allow;
        [ColorUsage(true, true)]
        public Color reject;
    }

    public class SlotMergeBaker : Baker<SlotMergeAuthoring>
    {
        public override void Bake(SlotMergeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var gridRectAuthorin = authoring.GetComponent<GridAuhoring>();
            AddComponent(entity, new SlotMergeTag());
            AddComponent(entity, new SlotColors() { current = (Vector4)authoring.current, allow = (Vector4)authoring.allow, reject = (Vector4)authoring.reject });
            AddComponent(entity, new MaterialColorProperty() { value = (Vector4)authoring.current });
            AddComponent(entity, new SlotMergeOutputData());
        }
    }
}