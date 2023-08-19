
using Game.Commander.Components;

using Unity.Entities;
using UnityEngine;

namespace Game.Commander.Authoring
{
    [AddComponentMenu("Game/Commander/Commander Tag")]
    public class CommanderTagAuthoring : MonoBehaviour
    {
        
    }

    public class CommanderTagBaker : Baker<CommanderTagAuthoring>
    {
        public override void Bake(CommanderTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<CommanderTag>(entity);
        }
    }
}