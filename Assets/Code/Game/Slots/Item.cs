using UnityEngine;

namespace Code.Game.Slots
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private EntityData m_EntityData;
        [SerializeField] private GameObject m_Entity;
        public EntityData entityData => m_EntityData;
        public GameObject entity => m_Entity;

        public Item SetName(string name)
        {
            gameObject.name = name;
            if (m_Entity != null) m_Entity.name = name;
            return this;
        }
        
        public Item SetEntityData(EntityData entityData)
        {
            m_EntityData = entityData;
            return this;
        }

        public Item SetEntity(GameObject entity)
        {
            m_Entity = entity;
            m_Entity.transform.SetParent(transform);
            m_Entity.transform.ResetAll();
            return this;
        }
    }
}