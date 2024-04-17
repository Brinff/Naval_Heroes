using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Code.Diagnostic
{
    public class DiagnosticWindowView : IDisposable
    {
        private VisualElement m_Root;
        private VisualElement m_Tabs;
        private VisualElement m_CurrentWindow;
        private Label m_Title;
        private VisualElement m_ScrollView;

        private IDiagnostic[] m_Windows;
        
        public class Tub
        {
            private string m_Name;
            private IDiagnostic[] m_Diagnostics;
        }

        public static DiagnosticWindowView CreateFormActiveScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var roots = activeScene.GetRootGameObjects();
            var windows = roots.SelectMany(x => x.GetComponentsInChildren<IDiagnostic>()).ToArray();
            return new DiagnosticWindowView(windows);
        }
        
        public DiagnosticWindowView(IDiagnostic[] windows)
        {
            m_Windows = windows;
        }
        
        private void OnSelectWindow(IDiagnostic window)
        {
            if (m_CurrentWindow != null)
            {
                m_ScrollView.Remove(m_CurrentWindow);
            }

            m_Title.text = window.path;
            m_CurrentWindow = window.CreateVisualTree();
            m_ScrollView.Add(m_CurrentWindow);
            Debug.Log($"Select window: {window.path}");
        }

        public VisualElement Create()
        {
            var tabbedView = new TabbedViewDiagnostic();
            
            var tabbedViewA = new TabbedViewDiagnostic();
            tabbedViewA.AddTab(new TabButtonDiagnostic("TestA", new Button(){ text = "Content A"}), true);
            tabbedViewA.AddTab(new TabButtonDiagnostic("TestB", new Button(){ text = "Content B"}), false);


            var visualElement = new VisualElement();
            for (int i = 0; i < 30; i++)
            {
                visualElement.Add(new Button(){ text = i.ToString()});
            }
            
            tabbedViewA.AddTab(new TabButtonDiagnostic("TestC", visualElement), false);
            
            
            tabbedView.AddTab(new TabButtonDiagnostic("TestA", tabbedViewA), true);
            tabbedView.AddTab(new TabButtonDiagnostic("TestB", new Button(){ text = "Content B"}), false);
            tabbedView.AddTab(new TabButtonDiagnostic("TestC", new Button(){ text = "Content C"}), false);
            /*m_Root = new VisualElement();
            m_Tabs = new VisualElement();
            m_Tabs.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            m_Tabs.style.flexWrap = new StyleEnum<Wrap>(Wrap.Wrap);
            m_Tabs.style.flexGrow = new StyleFloat(0f);
            m_Tabs.style.flexShrink = new StyleFloat(0f);
            foreach (var window in m_Windows)
            {
                var tub = new Button(() => OnSelectWindow(window));
                tub.text = window.title;
                m_Tabs.Add(tub);
            }

            m_Root.Add(m_Tabs);

            m_Title = new Label("No Selected");
            m_Title.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            m_Root.Add(m_Title);
            m_ScrollView = new ScrollView();
            m_ScrollView.style.flexGrow = new StyleFloat(0f);
            m_Root.Add(m_ScrollView);*/
            return tabbedView;
        }

        public void Dispose()
        {
            
        }
    }
}