using System;
using Code.Diagnostic;
using Code.Game.States;
using Code.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.IAP
{
    public class IAPShopDiagnostic : MonoBehaviour, IDiagnostic, IInitializable
    {
        private void OnEnable()
        {
            DiagnosticService.Register(this);
        }

        private void OnDisable()
        {
            DiagnosticService.Unregister(this);
        }

        public int order => 0;
        public string path => "IAP";
        public VisualElement CreateVisualTree()
        {
            var button = new Button(OnClick){ text = "Test IAP"};
            return button;
        }

        private void OnClick()
        {
            Debug.Log("Click");
            m_GameStateMachine.Play<BattleState>();
        }

        private GameStateMachine m_GameStateMachine;
        public void Initialize()
        {
            m_GameStateMachine = ServiceLocator.Get<GameStateMachine>();
        }
    }
}