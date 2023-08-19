using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentUtility
{
    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        if (component.TryGetComponent<T>(out T result))
        {
            return result;
        }
        else return component.gameObject.AddComponent<T>();
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent<T>(out T result))
        {
            return result;
        }
        else return gameObject.AddComponent<T>();
    }
}
