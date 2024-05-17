using Code.Services;
using Game.UI;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Shop
{
    public class ShopService : MonoBehaviour, IService, IInitializable
    {
        public void Initialize()
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