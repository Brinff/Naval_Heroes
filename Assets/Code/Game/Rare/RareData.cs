using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Rare")]
public class RareData : ScriptableObject, IData
{
    public Color color;
    public int id => name.GetDeterministicHashCode();
}
