using Game.Mission.Components;
using Unity.Entities;
using UnityEngine;

public class MissionCurrentAuthoring : MonoBehaviour
{
    public int mission;
}

public class MissionCurrentBaker : Baker<MissionCurrentAuthoring>
{
    public override void Bake(MissionCurrentAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new MissionCurrent() { value = authoring.mission });
    }
}