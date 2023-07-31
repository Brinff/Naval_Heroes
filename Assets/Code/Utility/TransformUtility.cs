using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility
{
    public static void ResetAll(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static void SetUniformScale(this Transform transform, float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
