using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Code.UI.Components;

namespace UI.Components
{
    public class TweenButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField]
        private TweenSequence m_Press;
        [SerializeField]
        private TweenSequence m_Release;

        public event UnityAction OnClick;
        public event UnityAction OnDown;
        public event UnityAction OnUp;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Press.Play(false);
            OnDown?.Invoke();      
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_Release.Play(false);
            OnUp?.Invoke();
        }
    }
}