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
    public class UIRoot : MonoBehaviour, IService
    {
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
    }
}
