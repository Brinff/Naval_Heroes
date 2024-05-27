using System;
using System.Linq;
using Assets.UI.Code.Widgets;
using Code.UI.Widgets.Stash;
using Code.Game.Slots.DragAndDrop;
using Code.Services;
using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Game.Slots.Stash
{
    public class StashMediator : MonoBehaviour, IService, IInitializable
    {
        private StashService m_StashService;
        private StashView m_StashView;
        private BottomLayoutWidget m_BottomLayoutWidget;
        private StashButtonWidget m_StashButtonWidget;
        private BuyLayoutView m_BuyLayoutView;

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
            m_StashService = ServiceLocator.Get<StashService>();
            m_StashView = ServiceLocator.Get<StashView>();
            m_BottomLayoutWidget = ServiceLocator.Get<UIRoot>().GetWidget<BottomLayoutWidget>();
            m_StashButtonWidget = ServiceLocator.Get<UIRoot>().GetWidget<StashButtonWidget>();
            m_BuyLayoutView = ServiceLocator.Get<BuyLayoutView>();

            m_StashButtonWidget.button.OnClick += OnClickOpenStashButton;
            m_StashView.closeButton.OnClick += OnClickCloseStashButton;

            m_StashButtonWidget.notificator.SetActive(m_StashService.isNeedInspect);
            m_StashService.OnUpdateInspect += UpdateInspect;

            Hide(true);
        }

        private void UpdateInspect()
        {
            m_StashButtonWidget.notificator.SetActive(m_StashService.isNeedInspect);
        }

        private void OnClickOpenStashButton()
        {
            Show(false);
        }

        private void OnClickCloseStashButton()
        {
            Hide(false);
        }

        public void Show(bool immedetatley)
        {
            m_StashView.slot.Fill(m_StashService.items.ToArray());
            m_StashView.Show(immedetatley);
            m_BuyLayoutView.Hide(immedetatley);
            m_BottomLayoutWidget.Hide(immedetatley);
            if (m_StashService.isNeedInspect) m_StashService.Inspected();
        }

        public void Hide(bool immedetatley)
        {
            m_StashView.slot.Clear();
            m_StashView.Hide(immedetatley);
            m_BuyLayoutView.Show(immedetatley);
            m_BottomLayoutWidget.Show(immedetatley);
        }
    }
}