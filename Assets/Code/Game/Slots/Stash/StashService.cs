using Code.Services;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Game.Slots.Stash
{
    public class StashService : MonoBehaviour, IService, IInitializable
    {
        [System.Serializable]
        public class StashItem
        {
            public string Id;
        }

        [SerializeField]
        private List<StashItem> m_StashItems = new List<StashItem>();

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public void Initialize()
        {
            
        }
    }
}