using System;
using Code.Game.UI.Components;
using Code.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Slots.Buy
{
    public class BuyShipView : MonoBehaviour, IService
    {
        [SerializeField] private Category m_Category;
        public Category category => m_Category;
        
        [SerializeField] private Slot m_Slot;
        public Slot slot => m_Slot;

        [SerializeField]
        private ValueLabel m_CostLabel;

        public ValueLabel costLabel => m_CostLabel;
        

        public void Show(bool immediately)
        {
            
        }

        public void Unlock(bool immediately)
        {
            
        }

        public void Hide(bool immediately)
        {
            
        }

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