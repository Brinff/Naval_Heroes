using Game.Eye.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Eye.Authoring
{
    [AddComponentMenu("Game/Eye/Main Camera Tag")]
    public class MainCameraTagAuthoring : MonoBehaviour
    {

    }

    public class MainCameraTagBaker : Baker<MainCameraTagAuthoring>
    {
        public override void Bake(MainCameraTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<MainCameraTag>(entity);
        }
    }
}