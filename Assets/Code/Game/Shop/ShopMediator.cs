using Code.Services;
using Game.UI;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Shop
{
    public class ShopMediator : MonoBehaviour, IService, IInitializable
    {
        private NavigateMenuWidget m_NavigateMenuWidget;
        private NavigateMenuItem m_NavigateMenuItem;
        public void Initialize()
        {
            m_NavigateMenuWidget = ServiceLocator.Get<UIController>().GetWidget<NavigateMenuWidget>();
            m_NavigateMenuItem = m_NavigateMenuWidget.items.First(x => x.name == "Shop");
            m_NavigateMenuItem.SetLock(false, false);
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