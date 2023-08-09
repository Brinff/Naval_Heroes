using Unity.Entities;
using UnityEngine;

using Game.Data.Components;

namespace Game.Data.Authoring
{
    [AddComponentMenu("Game/Data/Entity Data")]
    public class EntityDataAuthoring : MonoBehaviour
    {
        public EntityData data;
    }

    public class EntityDataBaker : Baker<EntityDataAuthoring>
    {
        public override void Bake(EntityDataAuthoring authoring)
        {          
            if (authoring.data)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddSharedComponent(entity, new EntityDataId() { value = authoring.data.id });
            }
        }
    }
}