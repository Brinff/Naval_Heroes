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
            [SerializeField]
            private int m_Id;

            public StashItem(int id)
            {
                m_Id = id;
            }
            public int id => m_Id;
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

        public void AddItem(EntityData ship)
        {
            m_StashItems.Add(new StashItem(ship.id));
        }
    }
}