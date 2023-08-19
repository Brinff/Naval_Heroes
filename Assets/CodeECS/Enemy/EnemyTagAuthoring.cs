using Unity.Entities;
using UnityEngine;

using Game.Enemy.Components;

namespace Game.Enemy.Authoring
{
    [AddComponentMenu("Game/Enemy/Enemy Tag")]
    public class EnemyTagAuthoring : MonoBehaviour
    {

    }

    public class EnemyTagAuthoringBaker : Baker<EnemyTagAuthoring>
    {
        public override void Bake(EnemyTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<EnemyTag>(entity);
        }
    }
}