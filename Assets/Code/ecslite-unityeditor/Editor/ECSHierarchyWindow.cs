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
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("ECS/Hierarchy")]
    public static void ShowExample()
    {
        ECSHierarchyWindow wnd = GetWindow<ECSHierarchyWindow>();
        wnd.titleContent = new GUIContent("ECSHierarchyWindow");
    }


    [SerializeField]
    private TreeViewState m_TreeViewState;
    private EntityTreeViewGUI m_EntityTreeViewGUI;


    private void CreateGUI()
    {
        if (EcsWorldViewSystem.views != null && EcsWorldViewSystem.views.Count > 0)
        {
            var viewSystem = EcsWorldViewSystem.views[0];
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();
            m_EntityTreeViewGUI = new EntityTreeViewGUI(viewSystem, m_TreeViewState);
        }
    }

    public void OnGUI()
    {
        if (m_EntityTreeViewGUI != null)
            m_EntityTreeViewGUI.OnGUI(new Rect(0, 0, position.width, position.height));
    }

    //private IMGUIContainer m_TreeViewContainer;
    //public void CreateGUI()
    //{
    //    VisualElement root = rootVisualElement;

    //    VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
    //    root.Add(labelFromUXML);
    //    //m_TreeViewContainer = root.Q<IMGUIContainer>("Body");
    //    //m_TreeViewContainer.onGUIHandler = OnDrawTree;

    //    if (EcsWorldViewSystem.views != null && EcsWorldViewSystem.views.Count > 0)
    //    {
    //        var viewSystem = EcsWorldViewSystem.views[0];

    //        var treeView = new EntityTreeView(viewSystem);
    //        root.Add(treeView);         
    //    }

    //    //if (m_TreeViewState == null)
    //    //    m_TreeViewState = new TreeViewState();

    //    //m_EntityTreeViewGUI = new EntityTreeViewGUI(m_TreeViewState);

    //}

    //private void OnDrawTree()
    //{

    //    //
    //    Rect rect = GUILayoutUtility.GetRect(10,600, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
    //    Debug.Log($"Rect: {rect}");
    //    if (m_EntityTreeViewGUI != null && m_TreeViewContainer != null) 
    //        m_EntityTreeViewGUI.OnGUI(rect);

    //    //GUILayout.Button("Test");
    //}
}
