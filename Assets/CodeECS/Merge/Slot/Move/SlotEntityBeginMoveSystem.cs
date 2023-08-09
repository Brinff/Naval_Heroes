
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Groups;
using Game.Merge.Components;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MoveGroup)), UpdateAfter(typeof(SlotMergeSystem))]
    public partial struct SlotEntityBeginMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (slotEntity, slotLocalTransform, pointerEndDragEvent) in SystemAPI.Query<RefRO<SlotEntity>, RefRO<LocalTransform>, RefRO<PointerBeginDragEvent>>())
            {
                if(slotEntity.ValueRO.value != Entity.Null)
                {
                    if (SystemAPI.HasComponent<SlotEntityPosition>(slotEntity.ValueRO.value)) SystemAPI.GetComponentRW<SlotEntityPosition>(slotEntity.ValueRO.value).ValueRW.value = slotLocalTransform.ValueRO.Position;
                    else beginEcb.AddComponent(slotEntity.ValueRO.value, new SlotEntityPosition() { value = slotLocalTransform.ValueRO.Position });
                }
            }
        }
    }
}

