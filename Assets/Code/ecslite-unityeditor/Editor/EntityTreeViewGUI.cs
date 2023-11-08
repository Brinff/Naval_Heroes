using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class EntityTreeViewGUI : TreeView
{
    private EcsWorld m_World;
    private EcsPool<Name> m_PoolName;
    private EcsPool<Childs> m_PoolChilds;
    private EcsPool<Root> m_PoolRoot;
    private List<TreeViewItem> m_Items = new List<TreeViewItem>();
    public EntityTreeViewGUI(EcsWorld world, TreeViewState treeViewState) : base(treeViewState)
    {
        m_World = world;
        Reload();
    }

    protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
    {
        return item?.displayName?.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    protected override TreeViewItem BuildRoot()
    {
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };

        m_Items.Clear();
        if (m_World != null && m_World.IsAlive())
        {
            m_PoolName = m_World.GetPool<Name>();
            m_PoolChilds = m_World.GetPool<Childs>();
            m_PoolRoot = m_World.GetPool<Root>();

            int[] entities = Array.Empty<int>();
            int count = m_World.GetAllEntities(ref entities);
            for (int i = 0; i < count; i++)
            {
                var entity = entities[i];
                if (!m_PoolRoot.Has(entity))
                {
                    m_Items.Add(new TreeViewItem(entity, 0, GetEntityName(m_World, entity)));
                    bool isHasChilds = m_PoolChilds.Has(entity);
                    if (isHasChilds)
                    {
                        ref var childs = ref m_PoolChilds.Get(entity);
                        if (childs.entities != null)
                        {
                            for (int c = 0; c < childs.entities.Length; c++)
                            {
                                var child = childs.entities[c];
                                if (child.Unpack(m_World, out int childEntity))
                                {
                                    m_Items.Add(new TreeViewItem(childEntity, 1, GetEntityName(m_World, childEntity)));
                                }
                            }
                        }
                    }
                }
            }
        }

        SetupParentsAndChildrenFromDepths(root, m_Items);
        return root;
    }

    private Type[] m_Types;
    private string GetEntityName(EcsWorld world, int entity)
    {
        var entityName = entity.ToString("X8");

        if (m_PoolName.Has(entity))
        {
            ref var name = ref m_PoolName.Get(entity);
            entityName = name.value;
        }
        else
        {
            int gen = m_World.GetEntityGen(entity);
            if (gen > 0)
            {
                var count = world.GetComponentTypes(entity, ref m_Types);
                for (var i = 0; i < count; i++)
                {
                    entityName = $"{entityName}:{EditorExtensions.GetCleanGenericTypeName(m_Types[i])}";
                }
            }
        }
        return entityName;
    }
}
