using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class HandleCallback<T> : IDisposable
{
    private HashSet<T> m_Items = null;

    public HandleCallback()
    {
        m_Items = new HashSet<T>();
        m_Execute = new List<T>();
    }

    public void Add(T item)
    {
        m_Items.Add(item);
    }

    public void Remove(T item)
    {
        m_Items.Remove(item);
    }

    public delegate void ExecuteDelegate<T0>(T0 item) where T0 : T;

    public bool HasAny()
    {
        return m_Items.Count > 0;
    }

    private List<T> m_Execute = null;

    public void Execute<T0>(ExecuteDelegate<T0> executeDelegate) where T0 : T
    {

        foreach (var item in m_Items)
        {
            if (item is T0) m_Execute.Add(item);
        }

        foreach (var item in m_Execute)
        {
            executeDelegate.Invoke((T0)item);
        }

        m_Execute.Clear();
    }

    public void Dispose()
    {
        m_Items.Clear();
    }
}