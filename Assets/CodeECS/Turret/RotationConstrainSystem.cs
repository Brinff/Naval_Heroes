using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

[UpdateAfter(typeof(TransformSystemGroup))]
[RequireMatchingQueriesForUpdate]
public partial struct RotationConstrainSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {

        foreach (var (rotationConstrainLookAtPoint, rotationConstrainLookAtEntity) in SystemAPI.Query<RefRW<RotationConstrainLookAtPointComponent>, RefRO<RotationConstrainLookAtEntityComponent>>())
        {
            if (SystemAPI.HasComponent<LocalTransform>(rotationConstrainLookAtEntity.ValueRO.value))
            {
                rotationConstrainLookAtPoint.ValueRW.value = SystemAPI.GetComponent<LocalToWorld>(rotationConstrainLookAtEntity.ValueRO.value).Position;
            }
        }

        foreach (var (transform, rotationConstrainTarget, rotationConstrainLookAt) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<RotationConstrainTargetComponent>, RefRO<RotationConstrainLookAtPointComponent>>())
        {
            float3 orgin = transform.ValueRO.Position;
            float3 direction = math.normalize(rotationConstrainLookAt.ValueRO.value - orgin);
            rotationConstrainTarget.ValueRW.rotation = quaternion.LookRotationSafe(direction, math.up());
        }

        foreach (var (transform, rotationConstrainTarget, rotationConstrainAxis, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationConstrainTargetComponent>, RefRO<RotationConstrainAxisComponent>>().WithEntityAccess())
        {
            float3 axis = rotationConstrainAxis.ValueRO.axis;
            quaternion rotation = rotationConstrainTarget.ValueRO.rotation;

            quaternion projector = quaternion.identity;
            if (SystemAPI.HasComponent<Parent>(entity))
            {
                projector = SystemAPI.GetComponentRO<LocalToWorld>(SystemAPI.GetComponent<Parent>(entity).Value).ValueRO.Rotation;
            }
            quaternion targetRotation = projectOnPlane(rotation, projector, axis);

            if (SystemAPI.HasComponent<RotationConstrainClampComponent>(entity))
            {
                var clamp = SystemAPI.GetComponentRO<RotationConstrainClampComponent>(entity);
                targetRotation = clampAxisAngle(targetRotation, quaternion.identity, clamp.ValueRO.minAngle, clamp.ValueRO.maxAngle, axis);
            }

            transform.ValueRW.Rotation = targetRotation;
        }
    }

    public static quaternion projectOnPlane(quaternion rotation, quaternion projector, float3 axis)
    {
        float3 forward = math.rotate(rotation, new float3(0, 0, 1));
        Vector3 project = math.rotate(math.inverse(projector), projectOnPlane(forward, math.rotate(projector, axis)));
        return quaternion.LookRotationSafe(project, new float3(0, 1, 0));
    }

    public static quaternion clampAxisAngle(quaternion rotation, quaternion projector, float min, float max, Vector3 axis)
    {
        quaternion newRotation = projectOnPlane(rotation, projector, axis);
        float3 forward = math.rotate(newRotation, new float3(0, 0, 1));
        float3 parentForward = math.rotate(projector, new float3(0, 0, 1));
        float angle = signedAngle(parentForward, forward, axis);
        angle = math.clamp(angle, -max, -min);
        return quaternion.AxisAngle(axis, math.radians(angle));
    }

    public static float angle(float3 from, float3 to)
    {
        float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
        if (num < math.EPSILON)
        {
            return 0f;
        }
        float num2 = Mathf.Clamp(math.dot(from, to) / num, -1f, 1f);
        return math.degrees(math.acos(num2));
    }

    public static float signedAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
        float num = angle(from, to);
        float num2 = from.y * to.z - from.z * to.y;
        float num3 = from.z * to.x - from.x * to.z;
        float num4 = from.x * to.y - from.y * to.x;
        float num5 = math.sign(axis.x * num2 + axis.y * num3 + axis.z * num4);
        return num * num5;
    }

    //public static float3 mul(quaternion rotation, float3 point)
    //{
    //    float num = rotation.value.x * 2f;
    //    float num2 = rotation.value.y * 2f;
    //    float num3 = rotation.value.z * 2f;
    //    float num4 = rotation.value.x * num;
    //    float num5 = rotation.value.y * num2;
    //    float num6 = rotation.value.z * num3;
    //    float num7 = rotation.value.x * num2;
    //    float num8 = rotation.value.x * num3;
    //    float num9 = rotation.value.y * num3;
    //    float num10 = rotation.value.w * num;
    //    float num11 = rotation.value.w * num2;
    //    float num12 = rotation.value.w * num3;
    //    Vector3 result = default(Vector3);
    //    result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
    //    result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
    //    result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
    //    return result;
    //}

    public static float3 projectOnPlane(float3 vector, float3 planeNormal)
    {
        float num = math.dot(planeNormal, planeNormal);
        if (num < math.EPSILON)
        {
            return vector;
        }
        float num2 = math.dot(vector, planeNormal);
        return new float3(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num);
    }
}
