using Code.UI.Components;
using DG.Tweening;
using Game.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.Widgets.Stash
{
    public class StashButtonWidget : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private TweenSequence m_ShowSequence;
        [SerializeField]
        private TweenSequence m_HideSequence;
        [SerializeField]
        private TweenButton m_Button;
        public TweenButton button => m_Button;
        [SerializeField]
        private GameObject m_Notificator;
        public GameObject notificator => m_Notificator;

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