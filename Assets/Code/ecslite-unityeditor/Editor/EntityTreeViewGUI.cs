using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EntityTreeViewGUI : TreeView
{
    private EcsWorldViewSystem m_WorldViewSystem;
    private EcsPool<Name> m_PoolName;
    private EcsPool<Root> m_PoolRoot;
    private List<TreeViewItem> m_Items = new List<TreeViewItem>();
    public EntityTreeViewGUI(EcsWorldViewSystem worldViewSystem, TreeViewState treeViewState) : base(treeViewState)
    {
        m_WorldViewSystem = worldViewSystem;
        m_Items = new List<TreeViewItem>();
        foreach (var entityView in m_WorldViewSystem.entities)
        {
            m_Items.Add(new TreeViewItem(entityView.GetHashCode(), 1, entityView.name));
        }
        m_WorldViewSystem.OnCreateViewEntity += OnCreateViewEntity;
        m_WorldViewSystem.OnDestroyViewEntity += OnDestroyViewEntity;
        m_WorldViewSystem.OnChangeViewEntity += OnChangeViewEntity;
        Reload();
    }

    private void OnChangeViewEntity(EcsViewEntity view)
    {
        var item = m_Items.Find(x => x.id == view.GetHashCode());
        item.displayName = view.name;
        Reload();
    }

    private void OnCreateViewEntity(EcsViewEntity view)
    {
        m_Items.Add(new TreeViewItem(view.GetHashCode(), 1, view.name));
        Reload();
    }

    private void OnDestroyViewEntity(EcsViewEntity view)
    {
        m_Items.RemoveAll(x => x.id == view.GetHashCode());
        Reload();
    }

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
        SetupParentsAndChildrenFromDepths(root, m_Items);
        return root;
    }


}
