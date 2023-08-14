

using Unity.Burst;
using Unity.Entities;

using Game.Pointer.Events;
using Game.Pointer.Groups;

namespace Game.Pointer.Systems
{
    [UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerRaycastGroup))]
    [BurstCompile]
    public partial struct PointerHoveredSystem : ISystem
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public void SendEvent<T>(EntityCommandBuffer ecb, Entity entity, RefRO<PointerId> pointerId) where T : unmanaged, IComponentData, IEnableableComponent
        //{
        //    if (SystemAPI.HasComponent<T>(entity))
        //    {
        //        RefRW<T> targetEvent = SystemAPI.GetComponentRW<T>(entity);
        //        if (!PointerHelper.HasFlag(targetEvent.ValueRW.value, pointerId.ValueRO.value)) targetEvent.ValueRW.value |= pointerId.ValueRO.value;
        //        ecb.SetComponentEnabled<T>(entity, true);
        //    }
        //}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (firstHoveredEntity, pointerId, bufferHoveredEntity) in SystemAPI.Query<RefRW<PointerFirstHoveredEntity>,RefRO<PointerId>, DynamicBuffer<PointerHoveredEntity>>())
            {
                int indexMin = -1;
                for (int i = 0; i < bufferHoveredEntity.Length; i++)
                {
                    if (indexMin < 0)
                    {
                        indexMin = i;
                    }
                    else
                    {
                        if (bufferHoveredEntity[indexMin].distance > bufferHoveredEntity[i].distance)
                        {
                            indexMin = i;
                        }
                    }
                }

                if (indexMin >= 0)
                {
                    if (firstHoveredEntity.ValueRO.value != bufferHoveredEntity[indexMin].entity)
                    {

                        if (SystemAPI.HasComponent<PointerExitEvent>(firstHoveredEntity.ValueRO.value))
                        {
                            RefRW<PointerExitEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerExitEvent>(firstHoveredEntity.ValueRO.value);
                            if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, pointerId.ValueRO.value)) pointerClickEvent.ValueRW.value |= pointerId.ValueRO.value;
                            beginEcb.SetComponentEnabled<PointerExitEvent>(firstHoveredEntity.ValueRO.value, true);
                        }

                        firstHoveredEntity.ValueRW.value = bufferHoveredEntity[indexMin].entity;

                        if (SystemAPI.HasComponent<PointerEnterEvent>(firstHoveredEntity.ValueRO.value))
                        {
                            RefRW<PointerEnterEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerEnterEvent>(firstHoveredEntity.ValueRO.value);
                            if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, pointerId.ValueRO.value)) pointerClickEvent.ValueRW.value |= pointerId.ValueRO.value;
                            beginEcb.SetComponentEnabled<PointerEnterEvent>(firstHoveredEntity.ValueRO.value, true);
                        }
                    }
                }
                else
                {
                    if(firstHoveredEntity.ValueRO.value != Entity.Null)
                    {
                        if (SystemAPI.HasComponent<PointerExitEvent>(firstHoveredEntity.ValueRO.value))
                        {                      
                            RefRW<PointerExitEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerExitEvent>(firstHoveredEntity.ValueRO.value);
                            if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, pointerId.ValueRO.value)) pointerClickEvent.ValueRW.value |= pointerId.ValueRO.value;
                            beginEcb.SetComponentEnabled<PointerExitEvent>(firstHoveredEntity.ValueRO.value, true);
                        }
                    }
                    firstHoveredEntity.ValueRW.value = Entity.Null;
                }
            }
        }
    }
}
