using Unity.Entities;
using UnityEngine;

using Game.Mission.Events;
using Game.Mission.Components;

namespace Game.Mission.Authoring
{
    [AddComponentMenu("Game/Mission/Listener/Mission Loaded Listener")]
    public class MissionLoadedListenerAuthoring : MonoBehaviour
    {

    }

    public class MissionLoadedListenerBaker : Baker<MissionLoadedListenerAuthoring>
    {
        public override void Bake(MissionLoadedListenerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<MissionLoadedEventListener>(entity);
        }
    }
}