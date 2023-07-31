using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string title;
    public Sprite sprite;
    public int[] cost;
}
