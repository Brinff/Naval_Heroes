using System;
using Code.Game.Entity;
using Code.Services;
using UnityEngine;

namespace Code.Game.Slots
{
    public class ItemFactory : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private EntityFactory m_EntityFactory;
        
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public void Destroy(Item item)
        {
            GameObject.Destroy(item.gameObject);
        }
        
        public Item Create(EntityData entityData)
        {
            Item item = new GameObject().AddComponent<Item>();
            return item.SetEntityData(entityData).SetEntity(m_EntityFactory.Create(entityData)).SetName(entityData.name);
        }

        public void Initialize()
        {
            m_EntityFactory = ServiceLocator.Get<EntityFactory>();
        }
    }
}