using System;
using Unity.Entities;

namespace Game.Data.Components
{
    public struct EntityDataId : ISharedComponentData, IQueryTypeParameter, IEquatable<EntityDataId>
    {
        public int value;

        public bool Equals(EntityDataId other)
        {
            return value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return value;
        }
    }
}

