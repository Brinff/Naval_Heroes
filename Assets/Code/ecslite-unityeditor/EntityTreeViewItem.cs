using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.IMGUI.Controls;
using Leopotam.EcsLite;

[Serializable]
public class EntityTreeElement : TreeViewItem
{
    public EntityTreeElement(EcsPackedEntityWithWorld entity) : base(entity.GetHashCode(), 0, entity.ToString())
    {

    }
}
