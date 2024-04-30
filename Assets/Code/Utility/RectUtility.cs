using System.Collections;
using UnityEngine;


public static class RectUtility
{

    public static Rect GetSignleRect(this RectInt rect)
    {
        return new Rect(rect.position, rect.size);
    }

    public static RectInt GetRoundedRect(this Rect rect)
    {
        return new RectInt(Vector2Int.RoundToInt(rect.position), Vector2Int.RoundToInt(rect.size));
    }

    public static Rect GetClampRect(this Rect rect, Rect other)
    {
        rect.x = Mathf.Clamp(rect.x, other.x, (other.x + other.size.x) - rect.size.x);
        rect.y = Mathf.Clamp(rect.y, other.y, (other.y + other.size.y) - rect.size.y);
        return rect;
    }

    public static RectInt GetIntersectRect(this RectInt rect, RectInt other)
    {
        var newRect = new RectInt();
        var min = Vector2Int.Max(rect.min, other.min);
        var max = Vector2Int.Min(rect.max, other.max);
        newRect.SetMinMax(min, max);
        return newRect;
    }
    
    public static Rect GetIntersectRect(this Rect rect, Rect other)
    {
        var newRect = new Rect();
        var min = Vector2.Max(rect.min, other.min);
        var max = Vector2.Min(rect.max, other.max);
        newRect.min = min;
        newRect.max = max;
        return newRect;
    }

    public static float GetArea(this Rect rect)
    {
        return rect.width * rect.height;
    }
}
