using Code.UI.Components;
using DG.Tweening;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigateMenuWidget : MonoBehaviour, IUIElement
{
    [SerializeField]
    private TweenSequence m_ShowSequence;
    [SerializeField]
    private TweenSequence m_HideSequence;

    [SerializeField]
    private List<NavigateMenuItem> m_Items = new List<NavigateMenuItem>();
    public IReadOnlyList<NavigateMenuItem> items => m_Items;

    private void OnEnable()
    {
        foreach (var item in m_Items)
        {
            item.OnClick += OnClickItem;
        }
    }

    private void OnDisable()
    {
        foreach (var item in m_Items)
        {
            item.OnClick -= OnClickItem;
        }
    }

    private void OnClickItem(NavigateMenuItem item)
    {
        Select(item, false);
    }

    public void Select(NavigateMenuItem item, bool immediately)
    {
        foreach (var localItem in m_Items)
        {
            localItem.SetSelect(localItem == item, immediately);
        }
    }

    public void Show(bool immediately)
    {
        gameObject.SetActive(true);
        m_ShowSequence.Play(immediately);
    }

    public void Hide(bool immediately)
    {
        m_HideSequence.Play(immediately).OnComplete(OnCompleteHide);
    }

    private void OnCompleteHide()
    {
        gameObject.SetActive(false);
    }
}
