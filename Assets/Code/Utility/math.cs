using System.Collections;
using Unity.Transforms;
using UnityEngine;

namespace Unity.Mathematics
{
    [System.Serializable]
    public struct float2x1
    {
        public float2 c0;
        public float2x1(float2 c0)
        {
            this.c0 = c0;
        }
    }


    //row 1 colum 2
    [System.Serializable]
    public struct float1x2
    {
        public float c0;
        public float c1;

        public float1x2(float c0, float c1)
        {
            this.c0 = c0;
            this.c1 = c1;
        }
    }

    public static partial class mathExtention
    {
        public static int repeat(int t, int length)
        {
            return (int)math.clamp(t - math.floor(t / length) * length, 0f, length);
        }

        public static float repeat(float t, float length)
        {
            return math.clamp(t - math.floor(t / length) * length, 0f, length);
        }

        public static bool approximately(float a, float b)
        {
            return math.abs(b - a) < math.max(1E-06f * math.max(math.abs(a), math.abs(b)), math.EPSILON * 8f);
        }
    }

    public class geometry
    {
        public static bool containsPointInBounds(float3 min, float3 max, float3 point)
        {
            return !math.any((point < min) | (max < point));
        }

        public static float3 getRayPoint(float3 orgin, float3 direction, float distance)
        {
            return orgin + direction * distance;
        }

        public static float planeDistance(float3 planePosition, float3 planeNormal)
        {
            planeNormal = math.normalize(planeNormal);
            return 0f - math.dot(planeNormal, planePosition);
        }

        public static bool raycastOnPlane(float3 rOrgin, float3 rDirection, float3 pPosition, float3 pNormal, out float distance)
        {
            return raycastOnPlane(rOrgin, rDirection, pNormal, planeDistance(pPosition, pNormal), out distance);
        }

        public static bool raycastOnPlane(float3 rOrgin, float3 rDirection, float3 pNormal, float pDistance, out float distance)
        {
            float num = math.dot(rDirection, pNormal);
            float num2 = 0f - math.dot(rOrgin, pNormal) - pDistance;

            if (mathExtention.approximately(num, 0f))
            {
                distance = 0;
                return false;
            }

            distance = num2 / num;

            return distance > 0f;
        }

        public static float2 projectPointAtLine(float2 point, float2 p0, float2 p1)
        {
            float2 relativePoint = point - p0;
            float2 lineDirection = p1 - p0;
            float length = math.length(lineDirection);
            float2 normalizedLineDirection = lineDirection;
            if (length > math.EPSILON)
                normalizedLineDirection /= length;

            float dot = math.dot(normalizedLineDirection, relativePoint);
            dot = math.clamp(dot, 0, length);

            return p0 + normalizedLineDirection * dot;
        }

        public static float3 projectPointAtLine(float3 point, float3 p0, float3 p1)
        {
            float3 relativePoint = point - p0;
            float3 lineDirection = p1 - p0;
            float length = math.length(lineDirection);
            float3 normalizedLineDirection = lineDirection;
            if (length > math.EPSILON)
                normalizedLineDirection /= length;

            float dot = math.dot(normalizedLineDirection, relativePoint);
            dot = math.clamp(dot, 0, length);

            return p0 + normalizedLineDirection * dot;
        }

        public struct Hits<T>
        {
            public byte count;
            public T point0, point1;
            public void Push(T val)
            {
                if (count == 0) { point0 = val; count++; }
                else if (count == 1) { point1 = val; count++; }
                //else print("This structure can only fit 2 values");
            }
        }


        public static float rayIntersectEllipse(float2 ro, float2 rd, float2 ab)
        {
            float2 ocn = ro / ab;
            float2 rdn = rd / ab;
            float a = math.dot(rdn, rdn);
            float b = math.dot(ocn, rdn);
            float c = math.dot(ocn, ocn);
            float h = b * b - a * (c - 1.0f);
            if (h < 0.0) return 0; // there's no intersection
            h = math.sqrt(h);
            float t1 = (-b - h) / a;
            float t2 = (-b + h) / a;
            // determine which (if any) of the solutions is the correct one
            if (math.max(t1, t2) < 0.0) return 0; // none is
            float t = (t1 > 0.0) ? t1 : t2;
            return t; // modified it to output the distance instead of the point
        }

        public static Hits<Vector2> lineIntersectEllepse(Vector2 p1, Vector2 p2, Vector2 ellepse)
        {
            Hits<Vector2> hits = default(Hits<Vector2>);
            Vector2 p3 = Vector2.zero;
            Vector2 p4 = Vector2.zero;
            Rect rect = default(Rect);
            {
                rect.xMin = Mathf.Min(p1.x, p2.x);
                rect.xMax = Mathf.Max(p1.x, p2.x);
                rect.yMin = Mathf.Min(p1.y, p2.y);
                rect.yMax = Mathf.Max(p1.y, p2.y);
            }
            float rx = ellepse.x;
            float ry = ellepse.y;
            float s = (p2.y - p1.y) / (p2.x - p1.x);
            float si = p2.y - (s * p2.x);
            float a = (ry * ry) + (rx * rx * s * s);
            float b = 2f * rx * rx * si * s;
            float c = rx * rx * si * si - rx * rx * ry * ry;

            float radicand_sqrt = Mathf.Sqrt((b * b) - (4f * a * c));
            p3.x = (-b - radicand_sqrt) / (2f * a);
            p4.x = (-b + radicand_sqrt) / (2f * a);
            p3.y = s * p3.x + si;
            p4.y = s * p4.x + si;

            if (rect.Contains(p3)) hits.Push(p3);
            if (rect.Contains(p4)) hits.Push(p4);
            return hits;

        }
    }

    public class mathMatrix
    {


        public static float mul(float1x2 a, float2x1 b)
        {
            return a.c0 * b.c0.x + a.c1 * b.c0.y;
        }

        public static float1x2 mul(float1x2 a, float2x2 b)
        {
            float c0 = a.c0 * b.c0.x + a.c1 * b.c0.y;
            float c1 = a.c0 * b.c1.x + a.c1 * b.c1.y;
            return new float1x2(c0, c1);
        }

        public static float2x1 mul(float2x2 a, float2x1 b)
        {
            float c0x = a.c0.x * b.c0.x + a.c1.x * b.c0.y;
            float c0y = a.c0.y * b.c0.x + a.c1.y * b.c0.y;
            return new float2x1(new float2(c0x, c0y));
        }

        public static float2x2 mul(float2x1 a, float1x2 b)
        {
            float c0x = a.c0.x * b.c0;
            float c1x = a.c0.x * b.c1;
            float c0y = a.c0.y * b.c0;
            float c1y = a.c0.y * b.c1;
            return new float2x2(new float2(c0x, c0y), new float2(c1x, c1y));
        }

        public static float2x1 transpose(float1x2 v)
        {
            return new float2x1(new float2(v.c0, v.c1));
        }

        public static float1x2 transpose(float2x1 v)
        {
            return new float1x2(v.c0.x, v.c0.y);
        }
    }
}