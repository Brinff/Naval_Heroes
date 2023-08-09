using Unity.Burst;
using Unity.Entities;

using Game.Merge.Groups;
using Game.Merge.Events;
using Game.Merge.Components;
using Unity.Transforms;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MergeGroup)), UpdateAfterAttribute(typeof(SlotMergeSystem))]
    public partial struct MergeApplySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (mergeApplyEvent, localToWorld, entity) in SystemAPI.Query<RefRO<MergeApplyEvent>, RefRO<LocalToWorld>>().WithEntityAccess())
            {
                var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
                var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

                endEcb.DestroyEntity(mergeApplyEvent.ValueRO.a);
                endEcb.DestroyEntity(mergeApplyEvent.ValueRO.b);

                var newEntity = beginEcb.Instantiate(mergeApplyEvent.ValueRO.result);
                beginEcb.AddComponent(newEntity, new SlotEntityPosition() { value = localToWorld.ValueRO.Position });
                beginEcb.SetComponent(newEntity, SystemAPI.GetComponent<LocalTransform>(mergeApplyEvent.ValueRO.b));
                beginEcb.SetComponent(entity, new SlotEntity() { value = newEntity });

                endEcb.RemoveComponent<MergeApplyEvent>(entity);
            }
        }
    }
}

