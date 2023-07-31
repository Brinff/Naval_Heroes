using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Entity", order = 0)]
public class EntityData : ScriptableObject
{
    [SerializeField]
    private GameObject m_Prefab;
    public GameObject prefab => m_Prefab;
}
