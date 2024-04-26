using Code.Services;
using UnityEngine;

namespace Code.Game.Slots.Merge
{
    public class MergeView : MonoBehaviour, IService
    {
        [SerializeField]
        private SlotGroup m_SlotGroup;
        public SlotGroup slotGroup => m_SlotGroup;
        [SerializeField] private Area m_Area;
        public Area area => m_Area;
        
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