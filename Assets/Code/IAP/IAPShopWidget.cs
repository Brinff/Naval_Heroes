using Game.UI;
using System.Collections;
using System.Linq;
using Code.Services;
using Code.UI.Components;
using DG.Tweening;
using UI.Components;
using UnityEngine;

namespace Code.IAP
{
    public class IAPShopWidget : MonoBehaviour, IUIElement
    {
        private IAPCategoryView[] m_Categories;
        
        [SerializeField]
        private TweenButton m_RestoreButton;
        public TweenButton restoreButton => m_RestoreButton;
        
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

        public T GetCategory<T>(IAPCategory category) where T : IAPCategoryView
        {
            if (m_Categories == null) m_Categories = GetComponentsInChildren<IAPCategoryView>();
            return m_Categories.First(x => x is T && x.category == category) as T;
        }
    }
}