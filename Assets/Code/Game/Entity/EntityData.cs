using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity", order = 0)]
public class EntityData : ScriptableObject
{
    public GameObject prefab;
    public int id => name.GetHashCode();
}
