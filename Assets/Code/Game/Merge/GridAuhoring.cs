
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Grid.Auhoring
{
    [AddComponentMenu("Game/Grid/Grid")]
    public class GridAuhoring : MonoBehaviour
    {
        public Vector2 center = Vector2.zero;
        public Vector2 scale = Vector2.one;
        public RectInt[] rects = new RectInt[1];

        public Rect GetSingleRect(RectInt rect)
        {
            Rect signleRect = new Rect();
            signleRect.position = rect.position * scale;
            signleRect.size = rect.size * scale;
            signleRect.position -= scale * 0.5f;
            signleRect.position += center;
            return signleRect;
        }

        public Matrix4x4 GetMatrix()
        {
            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            Matrix4x4 matrix = Matrix4x4.TRS(rotation * center, rotation, new Vector3(scale.x, scale.y, 1));
            return matrix;
        }

        public Matrix4x4 GetLocalToWorldMatrix()
        {
            return transform.localToWorldMatrix * GetMatrix();
        }

        public Matrix4x4 GetWorldToLocalMatrix()
        {
            return Matrix4x4.Inverse(GetLocalToWorldMatrix());
        }

        public Vector3 GetOffset()
        {
            Quaternion rotation = Quaternion.Euler(90, 0, 0);
            return rotation * -center;
        }


        public Rect GetProjectRect(RectInt rect, Matrix4x4 matrix, Matrix4x4 projectMatrix)
        {
            Rect projectGridRect = new Rect();

            Vector3 worldPosition = matrix.GetPosition();
            Vector3 worldSize = matrix.MultiplyVector((Vector2)rect.size);//math.mul(matrix, new float4(gridRect.size.x, gridRect.size.y, 0, 1)).xyz;

            Vector3 localPosition = projectMatrix.MultiplyPoint(worldPosition);
            Vector3 localSize = projectMatrix.MultiplyVector(worldSize);//MultiplyVector(projectMatrix, worldSize);
            
            localSize = Vector3Int.RoundToInt(localSize);

            if (localSize.x < 0)
            {
                localPosition.x += (localSize.x + 1);
            }
            if (localSize.y < 0)
            {
                localPosition.y += (localSize.y + 1);
            }
            
            projectGridRect.position = localPosition;
            projectGridRect.size = new Vector2((int)Mathf.Abs(localSize.x), (int)Mathf.Abs(localSize.y));

            return projectGridRect;
        }


    //public Vector2 test;
    private void OnDrawGizmos()
    {
        using (new GizmosScope(transform.localToWorldMatrix))
        {
            for (int i = 0; i < rects.Length; i++)
            {
                var rect = rects[i];

                for (int y = 0; y < rect.size.y; y++)
                {
                    for (int x = 0; x < rect.size.x; x++)
                    {
                        Gizmos.DrawWireCube(new Vector3(rect.position.x * scale.x + center.x + x * scale.x, 0, rect.position.y * scale.y + center.y + y * scale.y), new Vector3(scale.x, 0, scale.y));
                    }
                }
            }
        }
    }

    //public LocalBounds GetBounds(float height)
    //{
    //    Bounds localBounds = new Bounds(center, Vector3.zero);

    //    for (int i = 0; i < rects.Length; i++)
    //    {
    //        var rect = rects[i];

    //        for (int y = 0; y < rect.size.y; y++)
    //        {
    //            for (int x = 0; x < rect.size.x; x++)
    //            {
    //                Bounds bounds = new Bounds(new Vector3(rect.position.x * scale.x + center.x + x * scale.x, 0, rect.position.y * scale.y + center.y + y * scale.y), new Vector3(scale.x, height, scale.y));
    //                localBounds.Encapsulate(bounds);
    //            }
    //        }


    //    }

    //    return new LocalBounds() { min = localBounds.min, max = localBounds.max };
    //}
}

    //public class GridBaker : Baker<GridAuhoring>
    //{
    //    public override void Bake(GridAuhoring authoring)
    //    {
    //        var entity = GetEntity(TransformUsageFlags.Dynamic);
    //        AddComponents(authoring, entity, this, true);
    //        //AddComponent(entity, new GridScale() { value = authoring.scale });
    //        //AddComponent(entity, new GridCenter() { value = authoring.center });

    //        //var gridBuffer = AddBuffer<GridRect>(entity);
    //        //foreach (var item in authoring.rects)
    //        //{
    //        //    gridBuffer.Add(new GridRect() { position = new int2(item.position.x, item.position.y), size = new int2(item.size.x, item.size.y) });
    //        //}

    //        //quaternion rotation = quaternion.RotateX(math.radians(90));
    //        //float4x4 gridMatrix = float4x4.TRS(math.rotate(rotation, new float3(authoring.center, 0)), rotation, new float3(authoring.scale, 0));
    //        //AddComponent(entity, new GridMatrix() { value = gridMatrix });
    //    }

    //    public static void AddComponents(GridAuhoring authoring, Entity entity, IBaker baker, bool isAddRects)
    //    {
    //        baker.AddComponent(entity, new GridScale() { value = authoring.scale });
    //        baker.AddComponent(entity, new GridCenter() { value = authoring.center });


    //        var gridBuffer = baker.AddBuffer<GridRect>(entity);
    //        if (isAddRects)
    //        {
    //            foreach (var item in authoring.rects)
    //            {
    //                gridBuffer.Add(new GridRect() { position = new int2(item.position.x, item.position.y), size = new int2(item.size.x, item.size.y) });
    //            }
    //        }


    //        quaternion rotation = quaternion.RotateX(math.radians(90));
    //        float4x4 gridMatrix = float4x4.TRS(math.rotate(rotation, new float3(authoring.center, 0)), rotation, new float3(authoring.scale, 0));
    //        baker.AddComponent(entity, new GridMatrix() { value = gridMatrix });
    //    }
    //}
}
