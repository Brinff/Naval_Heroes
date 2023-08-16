using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Eye.Components;
using Unity.Transforms;
using Unity.Entities.Content;

namespace Game.Eye.Authoring
{

    [AddComponentMenu("Game/Camera")]
    public class CameraAuthoring : MonoBehaviour
    {
        public WeakObjectSceneReference linkCamera;

        public string cameraName;
        public Camera GetCamera()
        {
            foreach (var item in Camera.allCameras)
            {
                if (item.name == cameraName) return item;
            }
            return null;
        }

        private void OnDrawGizmosSelected()
        {
            using (new GizmosScope(transform.localToWorldMatrix))
            {
                var camera = GetCamera();
                if (camera != null)
                {
                    Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
                }
            }
        }
    }




    public class CameraBaker : Baker<CameraAuthoring>
    {
        public override void Bake(CameraAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<CameraTag>(entity);
            if (!string.IsNullOrEmpty(authoring.cameraName))
            {
                var camera = authoring.GetCamera();
                if (camera != null)
                {
                    AddComponentObject(entity, new CameraLink() { value = camera });
                    AddComponent(entity, new CameraFieldOfView() { value = camera.fieldOfView });
                }
            }
        }
    }
}


