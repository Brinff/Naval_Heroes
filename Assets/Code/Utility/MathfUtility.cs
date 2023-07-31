using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfUtility
{
    public static float Remap(float value, float minFrom, float maxFrom, float minTo, float maxTo)
    {
        return minTo + (value - minFrom) * (maxTo - minTo) / (maxFrom - minFrom);
    }
}
