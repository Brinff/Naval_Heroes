
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

using Game.Merge.Components;
using Game.Merge.Groups;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MoveGroup)), UpdateAfter(typeof(SlotEntityUpdateMoveSystem))]
    public partial struct SlotEntityEndMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (slotEntity, slotLocalTransform, pointerEndDragEvent) in SystemAPI.Query<RefRO<SlotEntity>, RefRO<LocalTransform>, RefRO<PointerEndDragEvent>>())
            {
                if(slotEntity.ValueRO.value != Entity.Null)
                {
                    if (SystemAPI.HasComponent<SlotEntityPosition>(slotEntity.ValueRO.value))
                    {
                        var enttityTransform = SystemAPI.GetComponentRW<SlotEntityPosition>(slotEntity.ValueRO.value);
                        enttityTransform.ValueRW.value = slotLocalTransform.ValueRO.Position;
                    }
                }
            }
        }
    }
}

