
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
    public partial struct SlotEntityTransformSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var (slotEntityPosition, localTransform) in SystemAPI.Query<RefRO<SlotEntityPosition>, RefRW<LocalTransform>>())
            {
                localTransform.ValueRW.Position = math.lerp(localTransform.ValueRW.Position, slotEntityPosition.ValueRO.value, deltaTime * 10);
            }
        }
    }
}

