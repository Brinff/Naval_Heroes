using Game.Utility;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/AbilityAmmo")]
public class AbilityAmmoData : ScriptableObject, IData
{
    public Sprite icon;
    public int id => name.GetDeterministicHashCode();
}
