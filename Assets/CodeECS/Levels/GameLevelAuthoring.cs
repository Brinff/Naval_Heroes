using Unity.Entities;
using UnityEngine;

[AddComponentMenu("Game/Game Level")]
public class GameLevelAuthoring : MonoBehaviour
{
    public int level;
}

public class GameLevelBaker : Baker<GameLevelAuthoring>
{
    public override void Bake(GameLevelAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new GameLevel() { value = authoring.level });
    }
}