using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Game.Eye.Components;
using Unity.Transforms;
using Unity.Entities.Content;
using Sirenix.OdinInspector;
using System;

namespace Game.Eye.Authoring
{

    [AddComponentMenu("Game/Eye/Camera")]
    public class CameraAuthoring : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            using (new GizmosScope(transform.localToWorldMatrix))
            {
                var camera = Camera.main;
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
        }
    }
}


