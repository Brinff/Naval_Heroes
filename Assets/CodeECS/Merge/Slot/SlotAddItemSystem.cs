using Game.Merge.Components;
using Game.Merge.Events;
using Game.Merge.Groups;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Game.Merge.Systems
{
    [BurstCompile, UpdateInGroup(typeof(MergeGroup)), UpdateAfter(typeof(EndItemGroup))]
    public partial struct SlotAddItemSystem : ISystem
    {

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (slotAddItemEvent, slotEntity) in SystemAPI.Query<RefRO<SlotAddItemEvent>>().WithEntityAccess())
            {
                var itemSlotParent = SystemAPI.GetComponentRW<ItemSlotParent>(slotAddItemEvent.ValueRO.itemEntity);
                itemSlotParent.ValueRW.value = slotEntity;
                var itemPosition = SystemAPI.GetComponentRW<ItemPosition>(slotAddItemEvent.ValueRO.itemEntity);
                itemPosition.ValueRW.value = slotAddItemEvent.ValueRO.position;
                var itemLocalPosition = SystemAPI.GetComponentRW<LocalTransform>(slotAddItemEvent.ValueRO.itemEntity);
                itemLocalPosition.ValueRW.Position = slotAddItemEvent.ValueRO.position;
                //Debug.DebugSystem.Log("add in add system");
                endEcb.RemoveComponent<SlotAddItemEvent>(slotEntity);
            }
        }
    }
}

