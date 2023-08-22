// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors {
    sealed class EcsPackedEntityListInspector : EcsComponentInspectorTyped<List<EcsPackedEntity>> {
        public override bool OnGuiTyped (string label, ref List<EcsPackedEntity> value, EcsEntityDebugView entityView) {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel (label);
            for (int i = 0; i < value.Count; i++)
            {
                if (value[i].Unpack(entityView.World, out var unpackedEntity))
                {
                    if (GUILayout.Button($"[{i}] Ping entity"))
                    {
                        EditorGUIUtility.PingObject(entityView.DebugSystem.GetEntityView(unpackedEntity));
                    }
                }
                else
                {
                    if (value[i].EqualsTo(default))
                    {
                        EditorGUILayout.SelectableLabel($"[{i}] <Empty entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                    }
                    else
                    {
                        EditorGUILayout.SelectableLabel($"[{i}] <Invalid entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                    }
                }
            }
            EditorGUILayout.EndVertical();
            return false;
        }
    }
}