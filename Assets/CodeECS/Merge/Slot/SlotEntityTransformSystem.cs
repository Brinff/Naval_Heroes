
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MergeGroup))]
    public partial struct ItemEntityTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (itemHandleEntity, itemPosition) in SystemAPI.Query<RefRO<ItemHandleEntity>, RefRO<ItemPosition>>())
            {
                var localTransform = SystemAPI.GetComponentRW<LocalTransform>(itemHandleEntity.ValueRO.value);
                localTransform.ValueRW.Position = math.lerp(localTransform.ValueRW.Position, itemPosition.ValueRO.value, deltaTime * 10);
            }
        }
    }
}

