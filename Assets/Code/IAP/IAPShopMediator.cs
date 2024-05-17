using Code.Services;
using Game.UI;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Code.IAP
{
    public class IAPShopMediator : MonoBehaviour, IService, IInitializable
    {
        private UIRoot m_UIRoot;
        private UICompositionController m_UICompositionController;

        private IAPShopWidget m_ShopWidget;
        private NavigateMenuWidget m_NavigateMenuWidget;
        private NavigateMenuItem m_NavigateMenuItem;

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
            m_UIRoot = ServiceLocator.Get<UIRoot>();
            m_UICompositionController = ServiceLocator.Get<UICompositionController>();

            m_NavigateMenuWidget = m_UIRoot.GetWidget<NavigateMenuWidget>();
            m_ShopWidget = m_UIRoot.GetWidget<IAPShopWidget>();

            m_NavigateMenuItem = m_NavigateMenuWidget.items.First(x => x.name == "Shop");
            m_NavigateMenuItem.SetLock(false, false);
            m_NavigateMenuItem.OnSelect += Show;
        }

        private void Show()
        {
            m_UICompositionController.Show<IAPShopComposition>();
        }
    }
}