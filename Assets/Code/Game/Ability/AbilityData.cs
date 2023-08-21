using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Ability")]
public class AbilityData : ScriptableObject, IData
{
    public Sprite icon;
    public int id => name.GetDeterministicHashCode();
}
