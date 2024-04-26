using Code.Services;
using UnityEngine;

namespace Code.Game.Slots.Battle
{
    public class BattleFieldService : MonoBehaviour, IService, IInitializable
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