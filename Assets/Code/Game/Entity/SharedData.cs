using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData
{
    private Dictionary<Type, ISharedData> datas;
    public SharedData(Transform transform)
    {
        var datas = transform.GetComponentsInChildren<ISharedData>();
        this.datas = new Dictionary<Type, ISharedData>();
        foreach (var data in datas)
        {
            if(this.datas.TryAdd(data.GetType(), data))
            {      
                data.As<ISharedInitalizeData>()?.InitalizeData();
            }
            else
            {
                Debug.LogError($"Has data: {data.GetType()}");
            }
        }
    }

    public T Get<T>() where T : ISharedData
    {
        if(datas.TryGetValue(typeof(T), out ISharedData data))
        {
            return (T)data;
        }
        return default(T);
    }
}
