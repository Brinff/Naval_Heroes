using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite.UnityEditor;
using System;
using System.Collections;
using UnityEditor.IMGUI.Controls;

public class ECSHierarchyWindow : EditorWindow
{
    [MenuItem("ECS/Hierarchy")]
    public static void ShowExample()
    {
        ECSHierarchyWindow wnd = GetWindow<ECSHierarchyWindow>();
        wnd.titleContent = new GUIContent("ECS Hierarchy");
    }

    [SerializeField]
    private int m_SelectedWorld = 0;
    [SerializeField]
    private TreeViewState m_TreeViewState;
    private EntityTreeViewGUI m_EntityTreeViewGUI;


    private Styles m_Style;
    private Styles style => m_Style != null ? m_Style : m_Style = new Styles();

    public class Styles
    {
        public GUIStyle toolbar = new GUIStyle("Toolbar");
        public GUIStyle toolbarDropDown = new GUIStyle("ToolbarDropDown");
        public GUIStyle toolbarSearchField = new GUIStyle("ToolbarSearchTextField");
    }

    private void CreateGUI()
    {
        if (EcsWorldViewSystem.views != null && EcsWorldViewSystem.views.Count > 0)
        {
            var viewSystem = EcsWorldViewSystem.views[0];
            viewSystem.OnCreateViewEntity += OnChangeEntity;
            viewSystem.OnChangeViewEntity += OnChangeEntity;
            viewSystem.OnDestroyViewEntity += OnChangeEntity;

            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_EntityTreeViewGUI = new EntityTreeViewGUI(viewSystem.world, m_TreeViewState);
        }
    }

    private void OnChangeEntity(int entity)
    {
        if (m_EntityTreeViewGUI != null) m_EntityTreeViewGUI.Reload();
    }

    public void OnGUI()
    {
        if (EcsWorldViewSystem.views != null && EcsWorldViewSystem.views.Count > 0 && m_EntityTreeViewGUI != null)
        {
            Rect toolbarRect = new Rect(0, 0, position.width, 22);
            GUILayout.BeginArea(toolbarRect);
            GUILayout.BeginHorizontal(style.toolbar);

            int selectIndex = EditorGUILayout.Popup(m_SelectedWorld, EcsWorldViewSystem.views.Select(x => x.name).ToArray(), style.toolbarDropDown);
            m_EntityTreeViewGUI.searchString = EditorGUILayout.TextField(m_EntityTreeViewGUI.searchString, style.toolbarSearchField);

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            Rect hierarchyRect = new Rect(0, toolbarRect.y + toolbarRect.height + 2, position.width, position.height - toolbarRect.height - 2);

            if (m_EntityTreeViewGUI != null)
                m_EntityTreeViewGUI.OnGUI(hierarchyRect);
        }
    }
}
