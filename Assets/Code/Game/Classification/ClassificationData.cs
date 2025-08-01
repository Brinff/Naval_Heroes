using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Classification")]
public class ClassificationData : ScriptableObject, IData
{
    public Sprite icon;
    public int id => name.GetDeterministicHashCode();
}
