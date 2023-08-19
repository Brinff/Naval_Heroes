using Unity.Entities;
using UnityEngine;
using Game.Spawn.Components;

namespace Game.Spawn.Authoring
{
    [AddComponentMenu("Game/Spawn/Spawner")]
    public class SpawnerAuthoring : MonoBehaviour
    {

    }

    public class SpawnerAuthoringBaker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<SpawnerTag>(entity);
        }
    }
}