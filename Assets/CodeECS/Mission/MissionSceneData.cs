using Unity.Entities;
using Unity.Entities.Serialization;

namespace Game.Mission.Components
{
    public struct MissionSceneData : IBufferElementData
    {
        public EntitySceneReference value;
    }
}

