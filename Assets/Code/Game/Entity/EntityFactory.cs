using System;
using Code.Services;
using UnityEngine;

namespace Code.Game.Entity
{
    public class EntityFactory : MonoBehaviour, IService
    {
        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public GameObject Create(EntityData entityData)
        {
           return Instantiate(entityData.prefab);
        }
    }
}