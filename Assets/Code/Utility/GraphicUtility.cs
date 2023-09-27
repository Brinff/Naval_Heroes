
using UnityEngine;
using UnityEngine.UI;

public static class GraphicUtility
{
    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }
}
