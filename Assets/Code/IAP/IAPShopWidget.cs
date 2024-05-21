using Game.UI;
using System.Collections;
using Code.Services;
using Code.UI.Components;
using DG.Tweening;
using UnityEngine;

namespace Code.IAP
{
    public class IAPShopWidget : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private TweenSequence m_ShowSequence;
        [SerializeField]
        private TweenSequence m_HideSequence;
        
        public void Hide(bool immediately)
        {
            
            m_HideSequence.Play(immediately).OnComplete(OnCompleteHide);
        }

        private void OnCompleteHide()
        {
            gameObject.SetActive(false);
        }

        public void Show(bool immediately)
        {
            gameObject.SetActive(true);
            m_ShowSequence.Play(immediately);
        }
    }
}