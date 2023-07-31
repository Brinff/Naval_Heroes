using System;
using UnityEngine;

public enum StatOperation { Overwrite, Add, Sub, Mul, Div }

public static class Stat
{
    public static float Apply(float a, float b, StatOperation operation)
    {
        switch (operation)
        {
            case StatOperation.Overwrite: return b;
            case StatOperation.Add: return a + b;
            case StatOperation.Sub: return a - b;
            case StatOperation.Mul: return a * b;
            case StatOperation.Div: return a / b;
        }
        return a;
    }
}