using Game.Mission.Components;
using Unity.Entities;
using Unity.Entities.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Game.Mission.Authoring
{
    public class MissionSceneDatabaseAuthoring : MonoBehaviour
    {

#if UNITY_EDITOR
        public SceneAsset[] scenes = new SceneAsset[0];
#endif

    }

    public class MissionSceneDatabaseBaker : Baker<MissionSceneDatabaseAuthoring>
    {
        public override void Bake(MissionSceneDatabaseAuthoring authoring)
        {
#if UNITY_EDITOR
            var entity = GetEntity(TransformUsageFlags.None);
            var buffer = AddBuffer<MissionSceneData>(entity);
            for (int i = 0; i < authoring.scenes.Length; i++)
            {
                var scene = authoring.scenes[i];
                buffer.Add(new MissionSceneData() { value = new EntitySceneReference(scene) });
            }
#endif
        }
    }
}