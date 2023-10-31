using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class EntityTreeView : TreeView
{
    private Type[] m_Types;
    private EcsWorldViewSystem m_WorldViewSystem;
    private EcsPool<Root> m_RootPool;

    public EntityTreeView(EcsWorldViewSystem worldViewSystem)
    {
        m_WorldViewSystem = worldViewSystem;
        fixedItemHeight = 18;
        makeItem = MakeItem;
        bindItem = BindItem;

        m_RootPool = m_WorldViewSystem.world.GetPool<Root>();

        SetRootItems(worldViewSystem.entities.Select(x=>new TreeViewItemData<EcsViewEntity>(x.GetHashCode(), x)).ToList());

        //foreach (var item in worldViewSystem.entities)
        //{

        //    //AddItem(new TreeViewItemData<EcsViewEntity>(item.Value.GetHashCode(), item.Value), 0, -1, true);
        //}
        
        m_WorldViewSystem.OnCreateViewEntity += OnCreateViewEntity;
        m_WorldViewSystem.OnDestroyViewEntity += OnDestroyViewEntity;
        //m_WorldViewSystem.OnChangeViewEntity += OnChangeViewEntity;
    }

    private void OnChangeViewEntity(EcsViewEntity view)
    {
        viewController.Move(view.GetHashCode(), view.parent, -1, true);
    }

    private void OnDestroyViewEntity(EcsViewEntity entity)
    {
        TryRemoveItem(entity.GetHashCode());
    }

    private void OnCreateViewEntity(EcsViewEntity entity)
    {
        AddItem(new TreeViewItemData<EcsViewEntity>(entity.GetHashCode(), entity), entity.parent, -1, true);
    }



    private VisualElement MakeItem()
    {
        return new Label();
    }

    private void BindItem(VisualElement element, int index)
    {
        var entityWorldView = GetItemDataForIndex<EcsViewEntity>(index);
        (element as Label).text = entityWorldView.name;
    }
}
