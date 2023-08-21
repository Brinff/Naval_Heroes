using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPreview(typeof(ViewAuthoring))]
public class ViewAuthoringPreview : ObjectPreview
{
    public static Dictionary<ViewAuthoring, Camera> s_Cameras = new Dictionary<ViewAuthoring, Camera>();

    public override void Cleanup()
    {
        ViewAuthoring viewAuthoring = target as ViewAuthoring;

        foreach (var item in s_Cameras)
        {
            Object.DestroyImmediate(item.Value.gameObject);
        }
        s_Cameras.Clear();

        //if (m_Camera != null)
        //{
        //    Object.DestroyImmediate(m_Camera.gameObject);
        //}
        //if (s_Cameras.)
        //{
        //    //var renderTexture = m_Camera.targetTexture;
        //    //m_Camera.targetTexture = null;
        //    //renderTexture.Release(); 
        //    //Object.DestroyImmediate(renderTexture);
        //    Object.DestroyImmediate(s_Cameras[viewAuthoring].gameObject);
        //    s_Cameras.Remove(viewAuthoring);
        //}
        base.Cleanup();
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }

    //private Camera m_Camera;

    public Camera GetCamera()
    {
        ViewAuthoring viewAuthoring = target as ViewAuthoring;

        if (s_Cameras.ContainsKey(viewAuthoring))
        {
            var camera = s_Cameras[viewAuthoring];
            camera.fieldOfView = viewAuthoring.fieldOfView;
            camera.transform.position = viewAuthoring.transform.position;
            camera.transform.rotation = viewAuthoring.transform.rotation;
            return camera;
        }

        GameObject cameraGameObject = new GameObject("[Preview Camera]");
        cameraGameObject.hideFlags = HideFlags.HideAndDontSave;
        var newCamera = cameraGameObject.AddComponent<Camera>();
        newCamera.fieldOfView = viewAuthoring.fieldOfView;
        newCamera.transform.position = viewAuthoring.transform.position;
        newCamera.transform.rotation = viewAuthoring.transform.rotation;
        newCamera.farClipPlane = 5000;

        s_Cameras.Add(viewAuthoring, newCamera);

        Vector2 size = GetMainGameViewSize();
        RenderTexture renderTexture = new RenderTexture((int)size.x, (int)size.y, 24);
        renderTexture.hideFlags = HideFlags.DontSave;
        newCamera.targetTexture = renderTexture;
        return newCamera;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        var camera = GetCamera();
        if (camera != null) GUI.DrawTexture(r, camera.targetTexture, ScaleMode.ScaleToFit);
    }
    public override bool HasPreviewGUI()
    {
        return true;
    }
}
