using Game;
using Unity.Entities;
using UnityEngine;

namespace Game.Battle.Authoring
{
    [AddComponentMenu("Game/Battle/Go Battle Listener")]
    public class GoBattleListenerAuthoring : MonoBehaviour
    {

    }

    public class GoBattleListenerAuthoringBaker : Baker<GoBattleListenerAuthoring>
    {
        public override void Bake(GoBattleListenerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<GoBattleListener>(entity);
        }
    }
}