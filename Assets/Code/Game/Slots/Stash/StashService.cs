using Code.Services;
using System.Collections.Generic;
using System.Linq;
using Code.IO;
using UnityEngine;

namespace Code.Game.Slots.Stash
{
    public class StashService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private EntityDatabase m_Database;
        
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
        
        private PlayerPrefsProperty<List<StashItem>> m_Items;
        private PlayerPrefsProperty<bool> m_IsNeedInspect;
        public bool isNeedInspect => m_IsNeedInspect.value;
        public IReadOnlyList<EntityData> items => m_Items.value.Select(x => m_Database.GetById(x.id)).ToList();

        public void Inspected()
        {
            m_IsNeedInspect.value = false;
        }
        
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
            m_Items = new PlayerPrefsProperty<List<StashItem>>(PlayerPrefsProperty.ToKey(nameof(StashService), nameof(m_Items)))
                .OnDefault(()=> new List<StashItem>())
                .Build();
            m_IsNeedInspect = new PlayerPrefsProperty<bool>(PlayerPrefsProperty.ToKey(nameof(StashService), nameof(m_IsNeedInspect)))
                .Build();
        }

        public void AddItem(EntityData entityData, bool isNew = false)
        {
            m_Items.value.Add(new StashItem(entityData.id));
            m_Items.Save();
            if (isNew)
            {
                m_IsNeedInspect.value = true;
            }
        }

        public bool RemoveItem(EntityData entityData)
        {
           var index = m_Items.value.FindIndex(x => x.id == entityData.id);
           if (index >= 0)
           {
               m_Items.value.RemoveAt(index);
               m_Items.Save();
               return true;
           }
           return false;
        }
    }
}