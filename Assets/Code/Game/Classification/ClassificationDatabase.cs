using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClassificationDatabase : Database<ClassificationData>
{
    //[SerializeField]
    //private ClassificationData[] m_Classifications;

    //private Dictionary<int, ClassificationData> m_ClassificationsMap;

    //public ClassificationData GetClassificationById(int id)
    //{
    //    if (m_ClassificationsMap == null)
    //    {
    //        m_ClassificationsMap = m_Classifications.ToDictionary(x => x.id);
    //    }
    //    if (m_ClassificationsMap.TryGetValue(id, out ClassificationData data)) return data;

    //    return null;
    //}
}
