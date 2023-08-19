using Game.Eye.Components;
using Unity.Entities;
using UnityEngine;

namespace Game.Eye.Authoring
{
    [AddComponentMenu("Game/Eye/Camera Link")]
    public class CameraLinkAuthoring : MonoBehaviour
    {
        public string cameraName;
        public Camera GetCamera()
        {
            foreach (var item in Camera.allCameras)
            {
                if (item.name == cameraName) return item;
            }
            return null;
        }
    }

    public class CameraLinkBaker : Baker<CameraLinkAuthoring>
    {
        public override void Bake(CameraLinkAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
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