using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct MergeGridObjectTransform : IAspect
{
    readonly RefRW<LocalTransform> objectLocalTransform;
    readonly RefRW<TargetPosition> objectTargetPosition;

    public float3 position { get => objectLocalTransform.ValueRO.Position; set => objectLocalTransform.ValueRW.Position = value; }
    public float3 targetPosition { get => objectTargetPosition.ValueRO.value; set => objectTargetPosition.ValueRW.value = value; }
}

