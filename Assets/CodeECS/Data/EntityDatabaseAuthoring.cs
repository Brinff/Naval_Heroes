using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

using Game.Data.Components;

namespace Game.Data.Authoring
{
    [AddComponentMenu("Game/Data/Entity Database")]
    public class EntityDatabaseAuthoring : MonoBehaviour
    {
        public List<EntityData> items = new List<EntityData>();
    }

    public class EntityDatabaseBaker : Baker<EntityDatabaseAuthoring>
    {
        public override void Bake(EntityDatabaseAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var buffer = AddBuffer<EntityDatabase>(entity);
            foreach (var item in authoring.items)
            {
                buffer.Add(new EntityDatabase() { id = item.id, prefab = GetEntity(item.prefab, TransformUsageFlags.None) });
            }
        }
    }
}