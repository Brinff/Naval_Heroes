using Code.Services;
using UnityEngine;

namespace Code.Game.Slots
{
    public class BuyShipMediator : MonoBehaviour, IService, IInitializable
    {
        [SerializeField]
        private ClassificationData m_Classification;
        public ClassificationData classification => m_Classification;
        
        private BuyShipService m_BuyShipService;
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
            m_BuyShipService = ServiceLocator.Get<BuyShipService>(x => x.classification == m_Classification);
        }
    }
}