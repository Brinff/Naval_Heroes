// ----------------------------------------------------------------------------
// The Proprietary or MIT-Red License
// Copyright (c) 2012-2022 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors {
    sealed class GameObjectList : EcsComponentInspectorTyped<List<GameObject>> {
        public override bool OnGuiTyped (string label, ref List<GameObject> value, EcsEntityDebugView entityView) {

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PrefixLabel(label);
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = EditorGUILayout.ObjectField(value[i], typeof(GameObject), false) as GameObject;
                //if (value[i].Unpack(entityView.World, out var unpackedEntity))
                //{
                //    if (GUILayout.Button($"[{i}] Ping entity"))
                //    {
                //        EditorGUIUtility.PingObject(entityView.DebugSystem.GetEntityView(unpackedEntity));
                //    }
                //}
                //else
                //{
                //    if (value[i].EqualsTo(default))
                //    {
                //        EditorGUILayout.SelectableLabel($"[{i}] <Empty entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                //    }
                //    else
                //    {
                //        EditorGUILayout.SelectableLabel($"[{i}] <Invalid entity>", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                //    }
                //}
            }
            EditorGUILayout.EndVertical();
            return true;
        }
    }
}