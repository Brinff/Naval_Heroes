using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private AbilityItem m_AbilityItem;
    [SerializeField]
    private RectTransform m_View;

    private List<AbilityItem> m_AbilityItems = new List<AbilityItem>();

    private void OnEnable()
    {
        m_AbilityItem.gameObject.SetActive(false);
    }

    public AbilityItem CreateAbility()
    {
        Debug.Log($"Create ability:");
        var instance = Instantiate(m_AbilityItem);
        instance.transform.SetParent(m_View);
        instance.gameObject.SetActive(true);
        m_AbilityItems.Add(instance);
        return instance;
    }

    public void Dispose(AbilityItem abilityItem)
    {
        if (m_AbilityItems.Remove(abilityItem))
        {
            abilityItem.Dispose();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < m_AbilityItems.Count; i++)
        {
            Destroy(m_AbilityItems[i].gameObject);
        }
        m_AbilityItems.Clear();
    }

    public void Hide(bool immediately)
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
    }
}
