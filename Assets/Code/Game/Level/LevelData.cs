using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Level")]
public class LevelData : ScriptableObject, IData
{
    public int reward = 1000;
    public List<GameObject> stages = new List<GameObject>();

    public int id => name.GetHashCode();
}
