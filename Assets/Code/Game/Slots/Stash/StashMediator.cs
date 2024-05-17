using System;
using Assets.UI.Code.Widgets;
using Assets.UI.Code.Widgets.Stash;
using Code.Game.Slots.DragAndDrop;
using Code.Services;
using Game.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.Game.Slots.Stash
{
    public class StashMediator : MonoBehaviour, IService, IInitializable
    {
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
            m_StashView = ServiceLocator.Get<StashView>();
            m_BottomLayoutWidget = ServiceLocator.Get<UIController>().GetWidget<BottomLayoutWidget>();
            m_StashButtonWidget = ServiceLocator.Get<UIController>().GetWidget<StashButtonWidget>();
            m_BuyLayoutView = ServiceLocator.Get<BuyLayoutView>();

            m_StashButtonWidget.button.OnClick += OnClickOpenStashButton;
            m_StashView.closeButton.OnClick += OnClickCloseStashButton;

            Hide(true);
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
            m_StashView.Show(immedetatley);
            m_BuyLayoutView.Hide(immedetatley);
            m_BottomLayoutWidget.Hide(immedetatley);
            m_StashView.slot
        }

        public void Hide(bool immedetatley)
        {
            m_StashView.Hide(immedetatley);
            m_BuyLayoutView.Show(immedetatley);
            m_BottomLayoutWidget.Show(immedetatley);
        }
    }
}