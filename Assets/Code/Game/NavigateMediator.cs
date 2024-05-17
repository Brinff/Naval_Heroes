using Code.Services;
using System.Collections;
using UnityEngine;
using Game.UI;

namespace Assets.Code.Game
{
    public class NavigateMediator : MonoBehaviour, IService, IInitializable
    {
        
        private NavigateMenuWidget m_NavigateMenuWidget;
        public void Initialize()
        {
            m_NavigateMenuWidget = ServiceLocator.Get<UIController>().GetWidget<NavigateMenuWidget>();
            foreach (var item in m_NavigateMenuWidget.items)
            {
                item.SetSelect(false, true);
                item.SetLock(true, true);
            }
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