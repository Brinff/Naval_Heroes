
using Code.Services;
using UnityEngine;

namespace Code.Game.Slots.Buy
{
    public class BuyAdsShipService : MonoBehaviour, IService, IInitializable
    {
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