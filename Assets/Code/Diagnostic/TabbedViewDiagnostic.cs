﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.Diagnostic
{
    public class TabbedViewDiagnostic : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TabbedViewDiagnostic, UxmlTraits>
        {
            
        }

        private const string k_styleName = "RuntimeTabbedViewStyle";
        private const string s_UssClassName = "unity-tabbed-view";
        private const string s_ContentContainerClassName = "unity-tabbed-view__content-container";
        private const string s_TabsContainerClassName = "unity-tabbed-view__tabs-container";

        private readonly VisualElement m_TabContent;
        private readonly VisualElement m_Content;

        private readonly List<TabButtonDiagnostic> m_Tabs = new List<TabButtonDiagnostic>();
        private TabButtonDiagnostic m_ActiveTab;

        public override VisualElement contentContainer => m_Content;

        public TabbedViewDiagnostic()
        {
            AddToClassList(s_UssClassName);

            styleSheets.Add(Resources.Load<StyleSheet>($"Styles/{k_styleName}"));

            m_TabContent = new VisualElement();
            m_TabContent.name = "unity-tabs-container";
            m_TabContent.AddToClassList(s_TabsContainerClassName);
            hierarchy.Add(m_TabContent);

            m_Content = new VisualElement();
            m_Content.name = "unity-content-container";
            m_Content.AddToClassList(s_ContentContainerClassName);
            hierarchy.Add(m_Content);

            RegisterCallback<AttachToPanelEvent>(ProcessEvent);
            
            /*AddTab(new TabButtonDiagnostic("Test Tab 1", new Button(){ text = "Test Content 1"}), true);
            AddTab(new TabButtonDiagnostic("Test Tab 2", new Button(){ text = "Test Content 2"}), false);*/
        }

        public void AddTab(TabButtonDiagnostic tabButton, bool activate)
        {
            m_Tabs.Add(tabButton);
            m_TabContent.Add(tabButton);

            tabButton.OnClose += RemoveTab;
            tabButton.OnSelect += Activate;

            if (activate)
            {
                Activate(tabButton);
            }
        }

        public void RemoveTab(TabButtonDiagnostic tabButton)
        {
            int index = m_Tabs.IndexOf(tabButton);

            // If this tab is the active one make sure we deselect it first...
            if (m_ActiveTab == tabButton)
            {
                DeselectTab(tabButton);
                m_ActiveTab = null;
            }

            m_Tabs.RemoveAt(index);
            m_TabContent.Remove(tabButton);

            tabButton.OnClose -= RemoveTab;
            tabButton.OnSelect -= Activate;

            // If we closed the active tab AND we have any tabs left - active the next valid one...
            if ((m_ActiveTab == null) && m_Tabs.Any())
            {
                int clampedIndex = Mathf.Clamp(index, 0, m_Tabs.Count - 1);
                TabButtonDiagnostic tabToActivate = m_Tabs[clampedIndex];

                Activate(tabToActivate);
            }
        }

        private void ProcessEvent(AttachToPanelEvent e)
        {
            // This code takes any existing tab buttons and hooks them into the system...
            for (int i = 0; i < m_Content.childCount; ++i)
            {
                VisualElement element = m_Content[i];

                if (element is TabButtonDiagnostic button)
                {
                    m_Content.Remove(element);

                    if (button.Target == null)
                    {
                        string targetId = button.TargetId;

                        button.Target = this.Q(targetId);
                    }

                    AddTab(button, false);
                    --i;
                }
                else
                {
                    element.style.display = DisplayStyle.None;
                }
            }

            // Finally, if we need to, activate this tab...
            if (m_ActiveTab != null)
            {
                SelectTab(m_ActiveTab);
            }
            else if (m_TabContent.childCount > 0)
            {
                m_ActiveTab = (TabButtonDiagnostic)m_TabContent[0];

                SelectTab(m_ActiveTab);
            }
        }

        private void SelectTab(TabButtonDiagnostic tabButton)
        {
            VisualElement target = tabButton.Target;

            tabButton.Select();
            if (target != null)
                Add(target);
        }

        private void DeselectTab(TabButtonDiagnostic tabButton)
        {
            VisualElement target = tabButton.Target;

            if (target != null)
                Remove(target);
            tabButton.Deselect();
        }

        public void Activate(TabButtonDiagnostic button)
        {
            if (m_ActiveTab != null)
            {
                DeselectTab(m_ActiveTab);
            }

            m_ActiveTab = button;
            SelectTab(m_ActiveTab);
        }
    }
}