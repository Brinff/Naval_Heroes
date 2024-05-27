using Code.Services;
using Code.UI.Components;
using DG.Tweening;
using Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuyLayoutView : MonoBehaviour, IService
{
    [SerializeField]
    private TweenSequence m_ShowSequence;
    [SerializeField]
    private TweenSequence m_HideSequence;
    [SerializeField]
    private List<SlotBuy> m_Slots = new List<SlotBuy>();

    

    private void OnEnable()
    {
        ServiceLocator.Register(this);
    }

    private void OnDisable()
    {
        ServiceLocator.Unregister(this);
    }

    public void Hide(bool immediately)
    {
        foreach (var slot in m_Slots)
        {
            slot.Hide(immediately);
        }
        //m_ShowSequence.Play(immediately).OnComplete(OnCompleteHide);
    }

    private void OnCompleteHide()
    {
        gameObject.SetActive(false);
    }

    public void Show(bool immediately)
    {
        foreach (var slot in m_Slots)
        {
            slot.Show(immediately);
        }
        //gameObject.SetActive(true);
        //m_ShowSequence.Play(immediately);
    }
}
