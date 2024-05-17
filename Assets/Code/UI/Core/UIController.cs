using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Code.Services;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Paterns;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace Game.UI
{
    public class UIController : MonoBehaviour, IService
    {
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private UICompositionModule m_CompositionModule;
        public UICompositionModule compositionModule => m_CompositionModule ? m_CompositionModule : m_CompositionModule = GetComponentInChildren<UICompositionModule>();

        private GraphicRaycaster m_GraphicRaycaster;
        private bool m_IsInteractable;

        private List<IUIElement> m_UIElemnts = new List<IUIElement>();

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public T GetWidget<T>() where T : IUIElement
        {
            if (m_UIElemnts.Count == 0) GetComponentsInChildren<IUIElement>(true, m_UIElemnts);
            var element = m_UIElemnts.Find(x => x is T);
            if (element != null) return (T)element;
            return default(T);
        }

        public bool isInteractable
        {
            get { return m_IsInteractable; }
            set
            {
                m_IsInteractable = value;
                if (m_GraphicRaycaster == null) m_GraphicRaycaster = GetComponentInChildren<GraphicRaycaster>();
                m_GraphicRaycaster.enabled = m_IsInteractable;
            }
        }

        public bool isRendered
        {
            get { return canvas.GetComponent<CanvasGroup>().alpha > 0; }
            set { canvas.GetComponent<CanvasGroup>().alpha = value ? 1 : 0; }
        }
    }
}
