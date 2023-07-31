using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtility
{
    public static Vector3 Abs(Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }


    public static bool IsPointInsideCountour(List<Vector2> contour, Vector2 point)
    {
        Vector2 p1, p2;
        bool inside = false;

        if (contour.Count < 3)
        {
            return false;
        }

        Vector2 oldPoint = new Vector2(contour[contour.Count - 1].x, contour[contour.Count - 1].y);

        for (int i = 0; i < contour.Count; i++)
        {
            Vector2 newPoint = new Vector2(contour[i].x, contour[i].y);

            if (newPoint.x > oldPoint.x)
            {
                p1 = oldPoint;
                p2 = newPoint;
            }
            else
            {
                p1 = newPoint;
                p2 = oldPoint;
            }

            if ((newPoint.x < point.x) == (point.x <= oldPoint.x) &&
               (point.y - p1.y) * (p2.x - p1.x) < (p2.y - p1.y) * (point.x - p1.x))
            {
                inside = !inside;
            }

            oldPoint = newPoint;
        }

        return inside;
    }


    public enum EndOfPathInstruction { Stop, Loop, Reverse }

    public struct TimeData
    {
        public int a;
        public int b;
        public float t;
    }

    public static TimeData GetPercentAtPathData(float[] times, float t, EndOfPathInstruction endOfPathInstruction)
    {
        // Constrain t based on the end of path instruction
        switch (endOfPathInstruction)
        {
            case EndOfPathInstruction.Loop:
                // If t is negative, make it the equivalent value between 0 and 1
                if (t < 0)
                {
                    t += Mathf.CeilToInt(Mathf.Abs(t));
                }
                t %= 1;
                break;
            case EndOfPathInstruction.Reverse:
                t = Mathf.PingPong(t, 1);
                break;
            case EndOfPathInstruction.Stop:
                t = Mathf.Clamp01(t);
                break;
        }

        int prevIndex = 0;
        int nextIndex = times.Length - 1;
        int i = Mathf.RoundToInt(t * (times.Length - 1)); // starting guess

        // Starts by looking at middle vertex and determines if t lies to the left or to the right of that vertex.
        // Continues dividing in half until closest surrounding vertices have been found.
        while (true)
        {
            // t lies to left
            if (t <= times[i])
            {
                nextIndex = i;
            }
            // t lies to right
            else
            {
                prevIndex = i;
            }
            i = (nextIndex + prevIndex) / 2;

            if (nextIndex - prevIndex <= 1)
            {
                break;
            }
        }

        float abPercent = Mathf.InverseLerp(times[prevIndex], times[nextIndex], t);
        return new TimeData() { a = prevIndex, b = nextIndex, t = abPercent };
    }

    public static bool IsQuadInsideCountour(List<Vector2> contour, Vector2 point, float size)
    {
        Vector2 p1, p2;
        bool inside = false;
        float halfSize = size / 2;
        Vector2 v0 = point + new Vector2(-halfSize, -halfSize);
        Vector2 v1 = point + new Vector2(-halfSize, halfSize);
        Vector2 v2 = point + new Vector2(halfSize, halfSize);
        Vector2 v3 = point + new Vector2(halfSize, -halfSize);

        if (contour.Count < 3)
        {
            return false;
        }

        Vector2 oldPoint = new Vector2(contour[contour.Count - 1].x, contour[contour.Count - 1].y);

        for (int i = 0; i < contour.Count; i++)
        {
            Vector2 newPoint = new Vector2(contour[i].x, contour[i].y);

            if (newPoint.x > oldPoint.x)
            {
                p1 = oldPoint;
                p2 = newPoint;
            }
            else
            {
                p1 = newPoint;
                p2 = oldPoint;
            }

            if (IsLineIntersect(p1, p2, v0, v1) || IsLineIntersect(p1, p2, v1, v2) || IsLineIntersect(p1, p2, v2, v3) || IsLineIntersect(p1, p2, v3, v0))
            {
                return true;
            }

            oldPoint = newPoint;
        }

        return inside;
    }
    public static bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector3 point)
    {
        point = Vector3.zero;
        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        if (doIntersect)
        {

            point = p1 + alphaNumerator * (p2 - p1) / alphaDenominator;
            return true;
        }
        return false;
    }

    public static Vector2? LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        Vector2? result = null;

        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        if (doIntersect)
        {

            result = p1 + alphaNumerator * (p2 - p1) / alphaDenominator;
        }
        return result;
    }

    public const float EPSILON = 0.0001f;

    public static bool RayIntersection(Vector3 AOrgin, Vector3 ADirection, Vector3 BOrign, Vector3 BDirection, out Vector3 intersection)
    {
        Vector3 lineVec3 = BOrign - AOrgin;
        Vector3 crossVec1and2 = Vector3.Cross(ADirection, BDirection);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, BDirection);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        if (Mathf.Abs(planarFactor) < EPSILON &&
            !(Mathf.Abs(crossVec1and2.sqrMagnitude) < EPSILON))
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = AOrgin + (ADirection * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }


    public static bool LineIntersection(Vector3 A0, Vector3 A1, Vector3 B0, Vector3 B1, out Vector3 intersection)
    {
        Vector3 aDiff = A1 - A0;
        Vector3 bDiff = B1 - B0;
        if (RayIntersection(A0, aDiff, B0, bDiff, out intersection))
        {
            float aSqrMagnitude = aDiff.sqrMagnitude;
            float bSqrMagnitude = bDiff.sqrMagnitude;

            if ((intersection - A0).sqrMagnitude <= aSqrMagnitude
                 && (intersection - A1).sqrMagnitude <= aSqrMagnitude
                 && (intersection - B0).sqrMagnitude <= bSqrMagnitude
                 && (intersection - B1).sqrMagnitude <= bSqrMagnitude)
            {
                return true;
            }

        }

        return false;
    }

    public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
    {
        v.x = Mathf.Clamp(v.x, min.x, max.x);
        v.y = Mathf.Clamp(v.y, min.y, max.y);
        v.z = Mathf.Clamp(v.z, min.z, max.z);
        return v;
    }

    public static Vector3 ProjectPointLineUnclamped(Vector3 point, Vector3 p0, Vector3 p1, out float distance, out float normalize)
    {
        Vector3 relativePoint = point - p0;
        Vector3 lineDirection = p1 - p0;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > Mathf.Epsilon)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        dot = Mathf.LerpUnclamped(0, length, dot / length);
        //dot = Mathf.Clamp(dot, 0, length);
        normalize = dot / length;
        Vector3 p = p0 + normalizedLineDirection * dot;
        distance = Vector3.Magnitude(p - point);
        return p;
    }

    public static Vector3 ProjectPointLine(Vector3 point, Vector3 p0, Vector3 p1, out float distance, out float normalize)
    {
        Vector3 relativePoint = point - p0;
        Vector3 lineDirection = p1 - p0;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > Mathf.Epsilon)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        //dot = Mathf.LerpUnclamped(0, length, dot / length);
        dot = Mathf.Clamp(dot, 0, length);
        normalize = dot / length;
        Vector3 p = p0 + normalizedLineDirection * dot;
        distance = Vector3.Magnitude(p - point);
        return p;
    }

    public static Vector3 ProjectPointLine(Vector3 point, Vector3 p0, Vector3 p1)
    {
        Vector3 relativePoint = point - p0;
        Vector3 lineDirection = p1 - p0;
        float length = lineDirection.magnitude;
        Vector3 normalizedLineDirection = lineDirection;
        if (length > Mathf.Epsilon)
            normalizedLineDirection /= length;

        float dot = Vector3.Dot(normalizedLineDirection, relativePoint);
        dot = Mathf.Clamp(dot, 0, length);

        return p0 + normalizedLineDirection * dot;
    }

    public static bool IsLineIntersect(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 s1, s2;
        s1.x = p1.x - p0.x;
        s1.y = p1.y - p0.y;
        s2.x = p3.x - p2.x;
        s2.y = p3.y - p2.y;

        float s, t;
        s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
        t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);

        return s > 0f && s < 1 && t > 0f && t < 1f;
    }

    public static bool IntersectSphere(Bounds bounds, Vector3 position, float radius)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        float ex = Mathf.Max(min.x - position.x, 0) + Mathf.Max(position.x - max.x, 0);
        float ey = Mathf.Max(min.y - position.y, 0) + Mathf.Max(position.y - max.y, 0);
        float ez = Mathf.Max(min.z - position.z, 0) + Mathf.Max(position.z - max.z, 0);

        return (ex < radius) && (ey < radius) && (ez < radius) && (ex * ex + ey * ey + ez * ez < radius * radius);
    }
    public static bool IntersectSphere(Bounds bounds, BoundingSphere sphere)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        Vector3 position = sphere.position;
        float radius = sphere.radius;

        float ex = Mathf.Max(min.x - position.x, 0) + Mathf.Max(position.x - max.x, 0);
        float ey = Mathf.Max(min.y - position.y, 0) + Mathf.Max(position.y - max.y, 0);
        float ez = Mathf.Max(min.z - position.z, 0) + Mathf.Max(position.z - max.z, 0);

        return (ex < radius) && (ey < radius) && (ez < radius) && (ex * ex + ey * ey + ez * ez < radius * radius);
    }

    public static bool PointContainsTriangle(Vector3 p, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 edge0 = v1 - v0;
        Vector3 edge1 = v2 - v1;
        Vector3 edge2 = v0 - v2;
        Vector3 C0 = p - v0;
        Vector3 C1 = p - v1;
        Vector3 C2 = p - v2;

        Vector3 v0v1 = v1 - v0;
        Vector3 v0v2 = v2 - v0;

        Vector3 N = Vector3.Cross(v0v1, v0v2);

        if (Vector3.Dot(N, Vector3.Cross(edge0, C0)) > 0 &&
            Vector3.Dot(N, Vector3.Cross(edge1, C1)) > 0 &&
            Vector3.Dot(N, Vector3.Cross(edge2, C2)) > 0) return true;

        return false;
    }

    public static bool SphereCastRay(BoundingSphere sphere, Ray ray, float lenght, out float distance)
    {
        return SphereCastRay(sphere.position, sphere.radius, ray.origin, ray.direction, lenght, out distance);
    }

    public static bool SphereCastRay(Vector3 spPosition, float spRadius, Vector3 rayOrgin, Vector3 rayDirection, float lenght, out float distance)
    {
        Vector3 p1 = rayOrgin + rayDirection * lenght;

        float sD = spRadius;
        VectorUtility.ProjectPointLine(spPosition, rayOrgin, p1, out float d, out float n);
        if (d <= sD)
        {
            float nD = Mathf.Clamp01(d / sD);
            float aSin = Mathf.Asin(nD);
            float cos = Mathf.Cos(aSin);
            distance = n * lenght - spRadius * cos * cos;
            return true;
        }

        distance = -1;
        return false;
    }

    public static bool SphereCastRaySphere(Vector3 spPosition, float spRadius, Vector3 rayOrgin, Vector3 rayDirection, float rayRadius, float lenght, out float distance)
    {
        Vector3 p1 = rayOrgin + rayDirection * lenght;

        float sD = spRadius + rayRadius;
        VectorUtility.ProjectPointLine(spPosition, rayOrgin, p1, out float d, out float n);
        if (d <= sD)
        {
            float nD = Mathf.Clamp01(d / sD);
            float aSin = Mathf.Asin(nD);
            float cos = Mathf.Cos(aSin);
            distance = n * lenght - spRadius * cos - rayRadius * cos;
            return true;
        }

        distance = -1;
        return false;
    }

    //private static bool SphereCast(List<BoundingSphere> spheres, Ray ray, float radius, float lenght, out float distance)
    //{

    //}


    public static bool RayTriangleIntersect(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, out float t)
    {
        return RayTriangleIntersect(ray.origin, ray.direction, v1, v2, v2, out t);
    }
    public static bool RayTriangleIntersect(Vector3 orig, Vector3 dir, Vector3 v0, Vector3 v1, Vector3 v2, out float t)
    {

        t = 0;
        // compute plane's normal
        Vector3 v0v1 = v1 - v0;
        Vector3 v0v2 = v2 - v0;
        // no need to normalize
        Vector3 N = Vector3.Cross(v0v1, v0v2); //v0v1.crossProduct(v0v2); // N 
        float area2 = N.magnitude;

        // Step 1: finding P

        // check if ray and plane are parallel ?
        float NdotRayDirection = Vector3.Dot(N, dir); // N.dotProduct(dir); 
        if (Mathf.Abs(NdotRayDirection) < Mathf.Epsilon) // almost 0 
            return false; // they are parallel so they don't intersect ! 

        // compute d parameter using equation 2
        float d = Vector3.Dot(-N, v0);

        // compute t (equation 3)
        t = -(Vector3.Dot(N, orig) + d) / NdotRayDirection;

        // check if the triangle is in behind the ray
        if (t < 0) return false; // the triangle is behind 

        // compute the intersection point using equation 1
        Vector3 P = orig + t * dir;

        // Step 2: inside-outside test
        Vector3 C; // vector perpendicular to triangle's plane 

        // edge 0
        Vector3 edge0 = v1 - v0;
        Vector3 vp0 = P - v0;
        C = Vector3.Cross(edge0, vp0);
        if (Vector3.Dot(N, C) < 0) return false; // P is on the right side 

        // edge 1
        Vector3 edge1 = v2 - v1;
        Vector3 vp1 = P - v1;
        C = Vector3.Cross(edge1, vp1);
        if (Vector3.Dot(N, C) < 0) return false; // P is on the right side 

        // edge 2
        Vector3 edge2 = v0 - v2;
        Vector3 vp2 = P - v2;
        C = Vector3.Cross(edge2, vp2);
        if (Vector3.Dot(N, C) < 0) return false; // P is on the right side; 

        return true; // this ray hits the triangle 
    }

    public static BoundingSphere TwoPointMinimumEnclosingSphere(Vector3 p0, Vector3 p1)
    {
        Vector3 center = (p0 + p1) * 0.5f;
        return new BoundingSphere(center, Vector3.Distance(center, p0));
    }

    public static BoundingSphere CircumSphere(Vector3 a, Vector3 b, Vector3 c)
    {
        //radius = 0;
        Plane plane = new Plane(a, b, c);
        Vector3 dirAB = b - a;
        Vector3 dirBC = c - b;
        Vector3 dirCA = a - c;

        Vector3 midPointAB = Vector3.Lerp(a, b, 0.5f);
        Vector3 midPointBC = Vector3.Lerp(b, c, 0.5f);
        Vector3 midPointCA = Vector3.Lerp(c, a, 0.5f);


        Vector3 perPointAB = Vector3.Cross(dirAB.normalized, -plane.normal);
        Vector3 perPointBC = Vector3.Cross(dirBC.normalized, -plane.normal);
        Vector3 perPointCA = Vector3.Cross(dirCA.normalized, -plane.normal);

        if (RayIntersection(midPointAB, perPointAB, midPointBC, perPointBC, out Vector3 center))
        {
            return new BoundingSphere(center, Vector3.Distance(center, b));
        }

        if (RayIntersection(midPointAB, perPointAB, midPointCA, perPointCA, out center))
        {
            return new BoundingSphere(center, Vector3.Distance(center, c));
        }

        return new BoundingSphere(Vector3.zero, 0);
    }

    public static bool IsEnclosingPoint(BoundingSphere sphere, Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector3.Distance(sphere.position, points[i]);
            if (!(dist < sphere.radius || Mathf.Approximately(dist, sphere.radius)))
            {
                return false;
            }
        }
        return true;
    }

    public static BoundingSphere MinimumEnclosingSphere(Vector3[] points)
    {
        if (points.Length < 2) return new BoundingSphere(Vector3.zero, 0);

        if (points.Length == 2)
        {        
            return TwoPointMinimumEnclosingSphere(points[0], points[1]);
        }

        BoundingSphere smallestSphere = new BoundingSphere(Vector3.zero, 100);
        BoundingSphere currentSphere;
        int countPoints = points.Length;
        for (int i = 0; i < countPoints; i++)
        {
            for (int j = i + 1; j < countPoints; j++)
            {
                currentSphere = TwoPointMinimumEnclosingSphere(points[i], points[j]);
                Gizmos.DrawWireSphere(currentSphere.position, currentSphere.radius);
                if (currentSphere.radius < smallestSphere.radius && IsEnclosingPoint(currentSphere, points))
                {
                    smallestSphere = currentSphere;
                }

                //for (int k = j + 1; k < countPoints; k++)
                //{
                //    currentSphere = CircumSphere(points[i], points[j], points[k]);
                //    Gizmos.DrawWireSphere(currentSphere.position, currentSphere.radius);
                //    if (currentSphere.radius < smallestSphere.radius && IsEnclosingPoint(currentSphere, points))
                //    {
                //        smallestSphere = currentSphere;
                //    }
                //}
            }
        }
        return smallestSphere;
    }
}
