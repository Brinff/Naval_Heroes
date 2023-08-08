
using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerRaycastGroup))]
[BurstCompile]
public partial struct PointerHoveredSortSystem : ISystemUpdate
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (firstHoveredEntity, bufferHoveredEntity) in SystemAPI.Query<RefRW<PointerFirstHoveredEntity>, DynamicBuffer<PointerHoveredEntity>>())
        {
            int indexMin = -1;
            for (int i = 0; i < bufferHoveredEntity.Length; i++)
            {
                var element = bufferHoveredEntity[i];
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

            if (indexMin >= 0) firstHoveredEntity.ValueRW.value = bufferHoveredEntity[indexMin].entity;
            else firstHoveredEntity.ValueRW.value = Entity.Null;
        }
    }
}
