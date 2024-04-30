using Code.Services;
using Game.Merge.Data;
using UnityEngine;

namespace Code.Game.Slots.Merge
{
    public class MergeService : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private MergeDatabase m_MergeDatabase;

        public bool IsPossibleMerge(EntityData a, EntityData b)
        {
            return m_MergeDatabase.GetResult(a, b) != null;
        }
        
        public EntityData Merge(EntityData a, EntityData b)
        {
            return m_MergeDatabase.GetResult(a, b);
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
            
        }
    }
}