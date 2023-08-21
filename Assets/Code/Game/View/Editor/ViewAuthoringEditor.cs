using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Linq;

[CustomEditor(typeof(ViewAuthoring), true)]
[CanEditMultipleObjects]
public class ViewAuthoringEditor : Editor
{

    private void OnEnable()
    {
        Repaint();
    }

    //private void OnDisable()
    //{
    //    EditorApplication.update -= Repaint;
    //}

    private void OnDestroy()
    {

        //ViewAuthoringPreview.s_Cameras.Clear();
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }

    private void OnSceneGUI()
    {
        foreach (var item in ViewAuthoringPreview.s_Cameras)
        {
            if (item.Value != null) CameraEditorUtils.DrawFrustumGizmo(item.Value);
        }
        Repaint();
    }




    //public override bool HasPreviewGUI()
    //{
    //    return true;
    //}

    //public override void DrawPreview(Rect previewArea)
    //{
    //    //List<string> names = targets.Select(x=>x.name).ToList();
    //    //var style = new GUIStyle("Box");
    //    //var rects = EditorGUIUtility.GetFlowLayoutedRects(previewArea, style, 10, 10, names);
    //    //for (int i = 0; i < rects.Count; i++)
    //    //{
    //    //    var rect = rects[i];
    //    //    GUI.Label(rect, names[i], style);
    //    //}
    //}
}
