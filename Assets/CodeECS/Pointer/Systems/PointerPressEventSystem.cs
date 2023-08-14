using Unity.Burst;
using Unity.Entities;
namespace Game.Pointer.Systems
{
    [UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerHoveredSystem))]
    [BurstCompile]
    public partial struct PointerPressEventSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var beginEcb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var endEcb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var (pressEntity, id, firstHoveredEntity) in SystemAPI.Query<RefRW<PointerPressEntity>, RefRO<PointerId>, RefRO<PointerFirstHoveredEntity>>().WithAll<PointerDownEvent>())
            {
                pressEntity.ValueRW.entity = firstHoveredEntity.ValueRO.value;
                if (SystemAPI.HasComponent<PointerDownEvent>(firstHoveredEntity.ValueRO.value))
                {
                    RefRW<PointerDownEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerDownEvent>(pressEntity.ValueRO.entity);
                    if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, id.ValueRO.value)) pointerClickEvent.ValueRW.value |= id.ValueRO.value;
                    beginEcb.SetComponentEnabled<PointerDownEvent>(pressEntity.ValueRO.entity, true);
                }
                //Debug.Log($"Press: {state.EntityManager.GetName(pressEntity.entity)}");
            }

            foreach (var (pressEntity, id, entity) in SystemAPI.Query<RefRO<PointerPressEntity>, RefRO<PointerId>>().WithAll<PointerUpEvent>().WithEntityAccess())
            {
                if (pressEntity.ValueRO.entity != Entity.Null)
                {
                    if (SystemAPI.HasComponent<PointerUpEvent>(pressEntity.ValueRO.entity))
                    {
                        RefRW<PointerUpEvent> pointerClickEvent = SystemAPI.GetComponentRW<PointerUpEvent>(pressEntity.ValueRO.entity);
                        if (!PointerHelper.HasFlag(pointerClickEvent.ValueRW.value, id.ValueRO.value)) pointerClickEvent.ValueRW.value |= id.ValueRO.value;
                        beginEcb.SetComponentEnabled<PointerUpEvent>(pressEntity.ValueRO.entity, true);
                    }
                    endEcb.SetComponent(entity, new PointerPressEntity());
                }
                //Debug.Log($"End Press: {state.EntityManager.GetName(pressEntity.entity)}");
            }
        }
    }
}
