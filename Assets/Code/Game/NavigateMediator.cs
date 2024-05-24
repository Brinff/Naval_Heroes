using Code.Services;
using System.Collections;
using System.Linq;
using UnityEngine;
using Game.UI;

namespace Code.Game
{
    public class NavigateMediator : MonoBehaviour, IService, IInitializable
    {
        private NavigateMenuWidget m_NavigateMenuWidget;

        public void Initialize()
        {
            m_NavigateMenuWidget = ServiceLocator.Get<UIRoot>().GetWidget<NavigateMenuWidget>();
            foreach (var item in m_NavigateMenuWidget.items)
            {
                item.SetSelect(false, true);
                item.SetLock(true, true);
            }
        }


        public bool Select(string menuName, bool immediately)
        {
            var menuItem = m_NavigateMenuWidget.items.FirstOrDefault(x => x.name == menuName);
            if (menuItem != null)
            {
                m_NavigateMenuWidget.Select(menuItem, immediately);
                return true;
            }

            return false;
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