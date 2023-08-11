using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Android;

[Flags]
public enum Pointer
{
    None = 0,
    Id1 = 1,
    Id2 = 2,
    Id3 = 4,
    Id4 = 8,
    Id5 = 16,
    Id6 = 32,
    Id7 = 64,
    Id8 = 128,
    Id9 = 256,
    Id10 = 512,
}



public static class PointerHelper
{

    public readonly static int[] s_Ids = new int[]
    {
        0,
        1 << 0,
        1 << 1,
        1 << 2,
        1 << 3,
        1 << 4,
        1 << 5,
        1 << 6,
        1 << 7,
        1 << 8,
        1 << 10
    };

    public static int GetPointerFromId(int id)
    {
        if (id >= 0) id += 1;
        int abs = math.abs(id);
        Debug.Log($"Pointer: {abs}");
        return s_Ids[abs];
    }

    public static bool HasFlag(int target, int value)
    {
        return (target & value) == value;
    }


}

