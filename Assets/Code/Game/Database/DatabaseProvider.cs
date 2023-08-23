using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseProvider : MonoBehaviour, IEcsDataProvider
{
    [SerializeField]
    private Database[] m_Databases = new Database[0];

    public IEcsData[] ProvideData()
    {
        return m_Databases;
    }
}
