using Game.Eye.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Eye.Authoring
{
    [AddComponentMenu("Game/Eye/Merge Camera Tag")]
    public class MergeCameraTagAuthoring : MonoBehaviour
    {

    }

    public class MergeCameraTagBaker : Baker<MergeCameraTagAuthoring>
    {
        public override void Bake(MergeCameraTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<MergeCameraTag>(entity);
        }
    }
}