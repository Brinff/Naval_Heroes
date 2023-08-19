using Game.Player.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Player.Authorign
{
    [AddComponentMenu("Game/Player/Player Tag")]
    public class PlayerTagAuthoring : MonoBehaviour
    {

    }

    public class PlayerTagBaker : Baker<PlayerTagAuthoring>
    {
        public override void Bake(PlayerTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<PlayerTag>(entity);
        }
    }
}