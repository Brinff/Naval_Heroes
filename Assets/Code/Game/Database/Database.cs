using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IData
{
    int id { get; }
}

public class Database<T> : MonoBehaviour, IEcsData where T : IData
{
    [SerializeField]
    private T[] m_Datas;

    private Dictionary<int, T> m_DatasMap;

    public T GetById(int id)
    {
        if (m_DatasMap == null)
        {
            m_DatasMap = m_Datas.ToDictionary(x => x.id);
        }
        if (m_DatasMap.TryGetValue(id, out T data)) return data;

        return default(T);
    }
}
