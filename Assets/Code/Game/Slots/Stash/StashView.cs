using Code.Services;
using UnityEngine;

namespace Code.Game.Slots.Stash
{
    public class StashView : MonoBehaviour, IService
    {
        [SerializeField] private SlotGroup m_SlotGroup;
        
        [SerializeField] private RectTransform m_Area;
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