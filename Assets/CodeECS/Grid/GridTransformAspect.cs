using System;
using System.Drawing;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct GridTransformAspect : IAspect
{

    readonly RefRO<GridMatrix> gridMatrix;
    readonly RefRO<LocalToWorld> localToWorld;
    readonly RefRW<LocalTransform> localTransform;
    readonly RefRO<GridCenter> gridCenter;
    readonly DynamicBuffer<GridRect> gridRects;

    public NativeArray<GridRect> GetRects()
    {
        return gridRects.AsNativeArray();
    }

    public GridRect GetRect()
    {
        GridRect newGridRect = new GridRect();
        for (int i = 0; i < gridRects.Length; i++)
        {
            GridRect gridRect = gridRects[i];

            if (i == 0)
            {
                newGridRect.position = gridRect.position;
                newGridRect.size = gridRect.size;
            }
            else newGridRect.SetMinMax(math.min(newGridRect.position, gridRect.position), math.max(newGridRect.position + newGridRect.size, gridRect.position + gridRect.size));
        }
        return newGridRect;
    }

    public float4x4 GetGridWorldToLocal()
    {
        return math.inverse(GetGridLocalToWorld());
    }

    public float4x4 GetGridLocalToWorld()
    {
        return math.mul(localToWorld.ValueRO.Value, gridMatrix.ValueRO.value);
    }

    public float3 GetOffset()
    {
        quaternion rotation = quaternion.RotateX(math.radians(90));
        return -math.rotate(rotation, new float3(gridCenter.ValueRO.value.x, gridCenter.ValueRO.value.y, 0));
    }

    public bool Raycast(float3 orgin, float3 direction, out float3 point)
    {
        float3 m_Normal = Vector3.Normalize(localToWorld.ValueRO.Up);
        float m_Distance = 0f - math.dot(m_Normal, localToWorld.ValueRO.Position);

        float num = Vector3.Dot(direction, m_Normal);
        float num2 = 0f - Vector3.Dot(orgin, m_Normal) - m_Distance;

        if (Approximately(num, 0f))
        {
            point = float3.zero;
            return false;
        }

        float enter = num2 / num;
        point = orgin + direction * enter;
        return enter > 0f;
    }

    public static bool Approximately(float a, float b)
    {
        return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), math.EPSILON * 8f);
    }

    public GridRect GetProjectRect(float3 point, float4x4 projectMatrix)
    {
        GridRect newGridRect = new GridRect();
        for (int i = 0; i < gridRects.Length; i++)
        {
            GridRect gridRect = gridRects[i];
            GridRect projectGridRect = new GridRect();

            

            float4x4 matrix = math.mul(float4x4.Translate(point), gridMatrix.ValueRO.value);

            float3 worldPosition = matrix.TransformPoint(float3.zero);
            float3 worldSize = matrix.TransformDirection(new float3(gridRect.size.x, gridRect.size.y, 0));//math.mul(matrix, new float4(gridRect.size.x, gridRect.size.y, 0, 1)).xyz;

            float3 localPosition = projectMatrix.TransformPoint(worldPosition);
            float3 localSize = projectMatrix.TransformDirection(worldSize);//MultiplyVector(projectMatrix, worldSize);
            localSize = math.round(localSize);

            if (localSize.x < 0)
            {
                localPosition.x += (localSize.x + 1);
            }
            if (localSize.y < 0)
            {
                localPosition.y += (localSize.y + 1);
            }

            projectGridRect.position = new float2(localPosition.x, localPosition.y);
            projectGridRect.size = new int2((int)math.abs(localSize.x), (int)math.abs(localSize.y));

            if (i == 0)
            {
                newGridRect.position = projectGridRect.position;
                newGridRect.size = projectGridRect.size;
            }
            else newGridRect.SetMinMax(math.min(newGridRect.position, projectGridRect.position), math.max(newGridRect.position + newGridRect.size, projectGridRect.position + projectGridRect.size));
        }

        return newGridRect;
    }

    public NativeArray<GridRect> GetProjectRects(float3 point, float4x4 projectMatrix, out GridRect boundsRect)
    {
        NativeArray<GridRect> rects = new NativeArray<GridRect>(gridRects.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
        boundsRect = new GridRect();
        for (int i = 0; i < gridRects.Length; i++)
        {
            GridRect gridRect = gridRects[i];
            GridRect projectGridRect = new GridRect();

            float4x4 matrix = math.mul(float4x4.Translate(point), gridMatrix.ValueRO.value);

            float3 worldPosition = matrix.TransformPoint(float3.zero);
            float3 worldSize = matrix.TransformDirection(new float3(gridRect.size.x, gridRect.size.y, 0));//math.mul(matrix, new float4(gridRect.size.x, gridRect.size.y, 0, 1)).xyz;

            float3 localPosition = projectMatrix.TransformPoint(worldPosition);
            float3 localSize = projectMatrix.TransformDirection(worldSize);//MultiplyVector(projectMatrix, worldSize);
            localSize = math.round(localSize);

            if (localSize.x < 0)
            {
                localPosition.x += (localSize.x + 1);
            }
            if (localSize.y < 0)
            {
                localPosition.y += (localSize.y + 1);
            }

            projectGridRect.position = new float2(localPosition.x, localPosition.y);
            projectGridRect.size = new int2((int)math.abs(localSize.x), (int)math.abs(localSize.y));

            if (i == 0)
            {
                boundsRect.position = projectGridRect.position;
                boundsRect.size = projectGridRect.size;
            }
            else boundsRect.SetMinMax(math.min(boundsRect.position, projectGridRect.position), math.max(boundsRect.position + boundsRect.size, projectGridRect.position + projectGridRect.size));

            rects[i] = projectGridRect;
        }

        return rects;
    }
}
