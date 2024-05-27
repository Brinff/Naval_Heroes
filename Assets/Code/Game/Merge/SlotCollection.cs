
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SlotCollection : MonoBehaviour
{
    private List<ISlot> slots = new List<ISlot>();
    [SerializeField]
    private RectTransform m_Layout;
    public delegate void ChangeDelegate(SlotCollection collection);

    public event ChangeDelegate OnChange;

    private bool m_IsDirty;

    public void SetDirty()
    {
        m_IsDirty = true;
    }

    public void RegisterSlot(ISlot slot)
    {
        if (!slots.Contains(slot))
        {
            slots.Add(slot);
            slot.Prepare(this);
        }
    }

    public void Prepare()
    {
        UpdateLayout();
        var slots = GetComponentsInChildren<ISlot>(true);
        foreach (var item in slots)
        {
            RegisterSlot(item);
        }
    }

    public void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_Layout);
    }

    public T[] GetSlots<T>() where T : ISlot
    {
        return slots.Where(x => x is T).Select(x => (T)x).ToArray<T>();
    }

    private void Update()
    {
        if (m_IsDirty)
        {
            OnChange?.Invoke(this);
            m_IsDirty = false;
        }
    }

}
