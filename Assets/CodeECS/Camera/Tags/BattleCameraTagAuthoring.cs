using Game.Eye.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Eye.Authoring
{
    [AddComponentMenu("Game/Eye/Battle Camera Tag")]
    public class BattleCameraTagAuthoring : MonoBehaviour
    {

    }

    public class BattleCameraTagBaker : Baker<BattleCameraTagAuthoring>
    {
        public override void Bake(BattleCameraTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<BattleCameraTag>(entity);
        }
    }
}