using Code.Services;
using Code.UI.Components;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.Slots.Stash
{
    public class StashView : MonoBehaviour, IService
    {
        [SerializeField] private TweenButton m_CloseButton;
        public TweenButton closeButton => m_CloseButton;

        [SerializeField] private StashSlot m_Slot;
        public StashSlot slot => m_Slot;
        
        [SerializeField] private RectTransform m_Area;

        [SerializeField]
        private TweenSequence m_ShowSequence;
        [SerializeField]
        private TweenSequence m_HideSequence;

        public void Show(bool immediately)
        {
            m_ShowSequence.Play(immediately);
        }

        public void Hide(bool immediately)
        {
            m_HideSequence.Play(immediately);
        }

        public RectTransform area => m_Area;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }
    }
}