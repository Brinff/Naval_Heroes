
using Unity.Mathematics;


public static class GridUtility
{
    public static GridRect GetRoundedRect(this GridRect rect)
    {
        return new GridRect() { position = math.round(rect.position), size = rect.size };
    }

    public static float2 GetMin(this GridRect rect)
    {
        return rect.position;
    }

    public static float2 GetMax(this GridRect rect)
    {
        return rect.position + rect.size;
    }

    public static void SetMinMax(this ref GridRect rect, float2 min, float2 max)
    {
        rect.position = min;
        float2 size = math.round(max - min);
        rect.size = new int2((int)size.x, (int)size.y);
    }

    public static GridRect GetIntersectRect(this GridRect rect, GridRect other)
    {
        var newRect = new GridRect();
        var min = math.max(rect.GetMin(), other.GetMin());
        var max = math.min(rect.GetMax(), other.GetMax());
        newRect.SetMinMax(min, max);
        return rect;
    }

    public static bool IsOverlap(this GridRect rect, GridRect other)
    {
        var rectMin = rect.GetMin();
        var rectMax = rect.GetMax();
        var otherMin = other.GetMin();
        var otherMax = other.GetMax();
        return math.all((rectMax > otherMin) & (rectMin < otherMax));
    }

    public static GridRect GetClampRect(this GridRect rect, GridRect other)
    {
        rect.position.x = math.clamp(rect.position.x, 0, other.size.x - rect.size.x);
        rect.position.y = math.clamp(rect.position.y, 0, other.size.y - rect.size.y);
        return rect;
    }
}
