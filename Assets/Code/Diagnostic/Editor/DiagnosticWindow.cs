using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Code.Diagnostic.Editor
{
    public class DiagnosticWindow : EditorWindow
    {
        [MenuItem("Mushroom/Game Diagnostic")]
        private static void ShowWindow()
        {
            var window = CreateWindow<DiagnosticWindow>();
            window.titleContent = new GUIContent("Game Diagnostic");
            window.Show();
        }

        private void OnEnable()
        {
            DiagnosticService.OnInitialize += OnInitialize;
            DiagnosticService.OnDispose += OnDispose;
        }

        private void OnDisable()
        {
            DiagnosticService.OnInitialize -= OnInitialize;
            DiagnosticService.OnDispose -= OnDispose;
            m_IsCreateGUI = false;
        }

        private void OnInitialize()
        {
            if (m_IsCreateGUI)
            {
                rootVisualElement.Add(DiagnosticService.CreateVisualTree());
            }
        }

        private void OnDispose()
        {
            rootVisualElement.Clear();
        }

        private bool m_IsCreateGUI;

        private void CreateGUI()
        {
            m_IsCreateGUI = true;
            if (DiagnosticService.isInitalized)
            {
                rootVisualElement.Add(DiagnosticService.CreateVisualTree());
            }
        }
    }
}