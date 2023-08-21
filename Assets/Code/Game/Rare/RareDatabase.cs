using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Android;

public class RareDatabase : Database<RareData>, IEcsData
{
    //[SerializeField]
    //private RareData[] m_Rares;
    //private Dictionary<int, RareData> m_RaresMap;

    //public RareData GetRareById(int id)
    //{
    //    if (m_RaresMap == null)
    //    {
    //        m_RaresMap = m_Rares.ToDictionary(x => x.id);
    //    }
    //    if (m_RaresMap.TryGetValue(id, out RareData data)) return data;

    //    return null;
    //}
}
