using Unity.Entities;
namespace Game.Data.Components
{
    public struct EntityDatabase : IBufferElementData
    {
        public int id;
        public Entity prefab;
    }
}

