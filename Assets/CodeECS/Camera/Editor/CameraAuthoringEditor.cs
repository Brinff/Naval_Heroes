using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Eye.Authoring;

namespace Game.Eye.Inspectors
{
    //[CustomEditor(typeof(CameraAuthoring))]
    public class CameraAuthoringEditor : Editor
    {
        private static Dictionary<CameraAuthoring, Camera> s_Cameras = new Dictionary<CameraAuthoring, Camera>();

        public bool HasCamera(CameraAuthoring target)
        {
            return s_Cameras.ContainsKey(target);
        }

        private Camera GetCamera(CameraAuthoring target)
        {
            if (HasCamera(target)) return s_Cameras[target];
            else
            {
                Camera camera = Instantiate(Camera.main);
                camera.tag = "Untagged";
                camera.name = "Camera Preview";
                DestroyImmediate(camera.GetComponent<AudioListener>());
                camera.targetTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24);
                s_Cameras.Add(target, camera);
                camera.transform.position = target.transform.position;
                camera.transform.rotation = target.transform.rotation;
                return camera;
            }
        }

        private void DisposeCamera(CameraAuthoring target)
        {
            if (s_Cameras.TryGetValue(target, out Camera camera))
            {
                var rt = camera.targetTexture;
                camera.targetTexture = null;
                rt.Release();
                DestroyImmediate(rt);
                DestroyImmediate(camera.gameObject);
                s_Cameras.Remove(target);
            }
        }

        private void OnEnable()
        {
            foreach (var item in targets)
            {
                GetCamera(item as CameraAuthoring);
            }
        }

        private void OnSceneGUI()
        {
            foreach (CameraAuthoring item in targets)
            {
                var camera = GetCamera(item);
                camera.transform.position = item.transform.position;
                camera.transform.rotation = item.transform.rotation;
            }
        }

        private void OnDisable()
        {
            foreach (var item in targets)
            {
                DisposeCamera(item as CameraAuthoring);
            }
        }

        private void OnDestroy()
        {
            foreach (var item in targets)
            {
                DisposeCamera(item as CameraAuthoring);
            }
        }

        public override bool HasPreviewGUI()
        {
            bool hasAny = false;
            foreach (var item in targets)
            {
                hasAny |= HasCamera(item as CameraAuthoring);
            }
            return hasAny;
        }

        public override void DrawPreview(Rect previewArea)
        {

            //Handles.BeginGUI();
            //GUILayout.BeginArea(previewArea);

            foreach (var item in targets)
            {
                var camera = GetCamera(item as CameraAuthoring);
                EditorGUI.DrawTextureTransparent(previewArea, camera.targetTexture, ScaleMode.ScaleToFit);

            }
            //Handles.EndGUI();
            //GUILayout.EndArea();
            //
            //Handles.BeginGUI(previewArea);
            //Handles.DrawCamera(previewArea, Camera.main);
            //base.DrawPreview(previewArea);
        }
    }
}
