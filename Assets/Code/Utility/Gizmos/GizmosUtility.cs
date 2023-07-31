using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosUtility
{
    public static void DrawPoint(Transform transform, Vector3 position, float size)
    {
        using (new GizmosScope(transform.localToWorldMatrix * Matrix4x4.Translate(position)))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.right * size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, Vector3.up * size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.forward * size);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, -Vector3.right * size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Vector3.zero, -Vector3.up * size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, -Vector3.forward * size);
        }
    }

    public static void DrawPolygon(Vector3 position, Quaternion rotation, float radius, int countSegments)
    {
        float p2 = Mathf.PI * 2;
        for (int i = 0; i < countSegments; i++)
        {
            float n0 = (float)i / countSegments;
            float n1 = (float)Mathf.Repeat(i + 1, countSegments) / countSegments;
            Vector3 p0 = position + rotation * new Vector3(Mathf.Sin(n0 * p2), Mathf.Cos(n0 * p2)) * radius;
            Vector3 p1 = position + rotation * new Vector3(Mathf.Sin(n1 * p2), Mathf.Cos(n1 * p2)) * radius;
            Gizmos.DrawLine(p0, p1);
        }
    }

    public static void DrawPolygon(Vector3 position, Quaternion rotation, Vector3 radius, int countSegments)
    {
        float p2 = Mathf.PI * 2;
        for (int i = 0; i < countSegments; i++)
        {
            float n0 = (float)i / countSegments;
            float n1 = (float)Mathf.Repeat(i + 1, countSegments) / countSegments;
            Vector3 p0 = position + rotation * new Vector3(Mathf.Sin(n0 * p2) * radius.x, Mathf.Cos(n0 * p2) * radius.y);
            Vector3 p1 = position + rotation * new Vector3(Mathf.Sin(n1 * p2) * radius.x, Mathf.Cos(n1 * p2) * radius.y);
            Gizmos.DrawLine(p0, p1);
        }
    }

    public static void DrawCircle(Vector3 position, Quaternion rotation, float radius)
    {
        DrawPolygon(position, rotation, radius, 24);
    }

    public static void DrawCircle(Vector3 position, Quaternion rotation, Vector3 radius)
    {
        DrawPolygon(position, rotation, radius, 24);
    }
}
