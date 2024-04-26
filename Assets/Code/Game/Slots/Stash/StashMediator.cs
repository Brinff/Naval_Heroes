using System;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Game.Slots.Stash
{
    public class StashMediator : MonoBehaviour, IService, IInitializable, IDropTarget
    {
        private StashView m_StashView;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public bool Overlap(IDragHandler dragHandler, out float weight)
        {
            weight = 0;
            return true;
        }

        public void Initialize()
        {
            m_StashView = ServiceLocator.Get<StashView>();
            
        }
    }
}