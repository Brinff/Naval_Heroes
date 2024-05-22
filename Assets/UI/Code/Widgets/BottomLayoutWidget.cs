using Code.UI.Components;
using DG.Tweening;
using Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.UI.Code.Widgets
{
    public class BottomLayoutWidget : MonoBehaviour, IUIElement
    {
        [SerializeField]
        private TweenSequence m_ShowSequence;
        [SerializeField]
        private TweenSequence m_HideSequence;

        public void Hide(bool immediately)
        {
            m_HideSequence.Play(immediately).OnComplete(OnCompleteHide);
        }

        public void Show(bool immediately)
        {
            gameObject.SetActive(true);
            m_ShowSequence.Play(immediately);
        }

        private void OnCompleteHide()
        {
            gameObject.SetActive(false);
        }
    }
}