using Game.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Projectile")]
public class ProjectileData : ScriptableObject, IData
{
    public int id => name.GetDeterministicHashCode();
}
