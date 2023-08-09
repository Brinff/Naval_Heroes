
using Unity.Entities;
using UnityEngine;

using Game.Merge.Data;
using Game.Merge.Components;

namespace Game.Merge.Authoring
{
    [AddComponentMenu("Game/Merge/Merge Entity Data")]
    public class MergeEntityDataAuthoring : MonoBehaviour
    {
        public MergeData data;
    }

    public class MergeEntityDataBaker : Baker<MergeEntityDataAuthoring>
    {
        public override void Bake(MergeEntityDataAuthoring authoring)
        {
            if (authoring.data)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                var buffer = AddBuffer<MergeEntityData>(entity);
                foreach (var item in authoring.data.items)
                {
                    if(item.isValid) buffer.Add(new MergeEntityData() { a = item.a.id, b = item.b.id, result = item.result.id });
                }
            }
        }
    }
}