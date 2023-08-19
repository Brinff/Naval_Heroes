using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using static GridMeshWireSystem;

[UpdateInGroup(typeof(GridRenderGroup))]
public partial class GridMeshWireSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //var ecb = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
        var renderDescription = new RenderMeshDescription(ShadowCastingMode.Off, false);

        var query = SystemAPI.QueryBuilder().WithAll<GridRenderer>().WithNone<GridMeshData>().Build();
        var entities = query.ToEntityArray(Allocator.Temp);
        for (int i = 0; i < entities.Length; i++)
        {
            var entity = entities[i];
            var renderer = EntityManager.GetSharedComponentManaged<GridRenderer>(entity);
            GridMeshData gridMeshData = new GridMeshData() { color = new float4(1,1,1,1) };
            gridMeshData.BeginFill();
            EntityManager.AddComponentObject(entity, gridMeshData);
            RenderMeshUtility.AddComponents(entity, EntityManager, renderDescription, new RenderMeshArray(new Material[] { renderer.material }, new Mesh[] { gridMeshData.EndFill() }), MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));
        }

        var buffers = SystemAPI.GetBufferLookup<GridRect>(true);
        foreach (var (gridMeshData, gridWireMesh, scale, center, entity) in SystemAPI.Query<GridMeshData, RefRO<GridMeshWire>, RefRO<GridScale>, RefRO<GridCenter>>().WithAll<GridRect>().WithEntityAccess())
        {
            if (buffers.TryGetBuffer(entity, out DynamicBuffer<GridRect> rects))
            {
                gridMeshData.width = gridWireMesh.ValueRO.width;
                gridMeshData.tails = gridWireMesh.ValueRO.tails;
                gridMeshData.center = center.ValueRO.value;
                gridMeshData.scale = scale.ValueRO.value;
                gridMeshData.BeginFill();
                for (int r = 0; r < rects.Length; r++)
                {
                    var e = rects[r];
                    gridMeshData.AddRect(e.position, e.size);
                }
                gridMeshData.EndFill();
            }
        }
    }


    public class GridMeshData : IComponentData
    {
        public float2 scale;
        public float2 center;
        public float width;
        public float tails;
        public float4 color;

        private NativeList<float3> vertices;
        private NativeList<float3> normals;
        private NativeList<float4> colors;
        private NativeList<int> triangles;

        private Mesh mesh;

        public void BeginFill()
        {
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = $"Grid Mesh";
                mesh.hideFlags = HideFlags.DontSave;
            }

            mesh.Clear();

            vertices = new NativeList<float3>(Allocator.Temp);
            normals = new NativeList<float3>(Allocator.Temp);
            colors = new NativeList<float4>(Allocator.Temp);
            triangles = new NativeList<int>(Allocator.Temp);
        }

        public void AddRect(float2 position, int2 size)
        {
            float3 orgin = new float3(position.x * scale.x, 0, position.y * scale.y);
            float3 halfScale = new float3(scale.x, 0, scale.y) * 0.5f;
            for (int x = 0; x < size.x + 1; x++)
            {
                float3 originA = orgin + new float3(x * scale.x, 0, 0) + new float3(center.x, 0, center.y - width) - halfScale;

                float sizeY = size.y * scale.y + width * 2;
                AddLine(originA, math.forward(), sizeY, color, color, width);
                if (tails > 0)
                {
                    float3 originB = originA + math.forward() * sizeY;
                    AddLine(originA, -math.forward(), tails, color, float4.zero, width);
                    AddLine(originB, math.forward(), tails, color, float4.zero, width);
                }
            }

            for (int y = 0; y < size.y + 1; y++)
            {
                float3 originA = orgin + new float3(0, 0, y * scale.y) + new float3(center.x - width, 0, center.y) - halfScale;
                float sizeX = size.x * scale.x + width * 2;
                AddLine(originA, math.right(), sizeX, color, color, width);

                if (tails > 0)
                {
                    float3 originB = originA + math.right() * sizeX;
                    AddLine(originA, -math.right(), tails, color, float4.zero, width);
                    AddLine(originB, math.right(), tails, color, float4.zero, width);
                }
            }
        }

        public Mesh EndFill()
        {
            mesh.SetVertices(vertices.AsArray());
            mesh.SetIndices(triangles.AsArray(), MeshTopology.Triangles, 0);
            mesh.SetNormals(normals.AsArray());
            mesh.SetColors(colors.AsArray());
            mesh.RecalculateBounds();

            vertices.Dispose();
            triangles.Dispose();
            normals.Dispose();
            colors.Dispose();

            return mesh;
        }

        private void AddLine(float3 start, float3 direction, float size, float4 colorStart, float4 colorEnd, float width)
        {
            AddLine(start, start + direction * size, colorStart, colorEnd, width);
        }

        private void AddLine(float3 start, float3 end, float4 colorStart, float4 colorEnd, float width)
        {
            int index = vertices.Length;
            float3 direction = math.normalize(end - start);
            float3 right = math.cross(direction, math.up());
            float3 p0 = start + right * width;
            float3 p1 = end + right * width;
            float3 p2 = end - right * width;
            float3 p3 = start - right * width;

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);
            vertices.Add(p3);

            colors.Add(colorStart);
            colors.Add(colorEnd);
            colors.Add(colorEnd);
            colors.Add(colorStart);

            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            triangles.Add(index + 0);
            triangles.Add(index + 1);
            triangles.Add(index + 2);

            triangles.Add(index + 2);
            triangles.Add(index + 3);
            triangles.Add(index + 0);
        }
    }

    //public static Mesh GetMesh(float width, float tails, Color color, float2 scale, float2 center, float2 size)
    //{
    //    Mesh mesh = new Mesh();
    //    mesh.name = $"Grid Mesh {size.x}x{size.y}";
    //    mesh.hideFlags = HideFlags.DontSave;

    //    List<Vector3> vertices = new List<Vector3>();
    //    List<int> triangles = new List<int>();
    //    List<Vector3> normals = new List<Vector3>();
    //    List<Color> colors = new List<Color>();

    //    for (int x = 0; x < size.x + 1; x++)
    //    {
    //        Vector3 originA = new Vector3(x * scale.x, 0, 0) + new Vector3(center.x, 0, center.y - width) - new Vector3(scale.x, 0, scale.y) * 0.5f;

    //        float sizeY = size.y * scale.y + width * 2;
    //        AddLine(originA, Vector3.forward, sizeY, color, color, width, ref vertices, ref triangles, ref normals, ref colors);
    //        if (tails > 0)
    //        {
    //            Vector3 originB = originA + Vector3.forward * sizeY;
    //            AddLine(originA, -Vector3.forward, tails, color, Color.clear, width, ref vertices, ref triangles, ref normals, ref colors);
    //            AddLine(originB, Vector3.forward, tails, color, Color.clear, width, ref vertices, ref triangles, ref normals, ref colors);
    //        }
    //    }

    //    for (int y = 0; y < size.y + 1; y++)
    //    {
    //        Vector3 originA = new Vector3(0, 0, y * scale.y) + new Vector3(center.x - width, 0, center.y) - new Vector3(scale.x, 0, scale.y) * 0.5f;
    //        float sizeX = size.x * scale.x + width * 2;
    //        AddLine(originA, Vector3.right, sizeX, color, color, width, ref vertices, ref triangles, ref normals, ref colors);

    //        if (tails > 0)
    //        {
    //            Vector3 originB = originA + Vector3.right * sizeX;
    //            AddLine(originA, -Vector3.right, tails, color, Color.clear, width, ref vertices, ref triangles, ref normals, ref colors);
    //            AddLine(originB, Vector3.right, tails, color, Color.clear, width, ref vertices, ref triangles, ref normals, ref colors);
    //        }
    //    }

    //    mesh.SetVertices(vertices);
    //    mesh.SetTriangles(triangles, 0);
    //    mesh.SetNormals(normals);
    //    mesh.SetColors(colors);
    //    mesh.RecalculateBounds();

    //    return mesh;
    //}

    //public static void AddLine(Vector3 start, Vector3 direction, float size, Color colorStart, Color colorEnd, float width, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector3> normals, ref List<Color> colors)
    //{
    //    AddLine(start, start + direction * size, colorStart, colorEnd, width, ref vertices, ref triangles, ref normals, ref colors);
    //}

    //public static void AddLine(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, float width, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector3> normals, ref List<Color> colors)
    //{
    //    int index = vertices.Count;
    //    Vector3 direction = Vector3.Normalize(end - start);
    //    Vector3 right = Vector3.Cross(direction, Vector3.up);
    //    Vector3 p0 = start + right * width;
    //    Vector3 p1 = end + right * width;
    //    Vector3 p2 = end - right * width;
    //    Vector3 p3 = start - right * width;

    //    vertices.Add(p0);
    //    vertices.Add(p1);
    //    vertices.Add(p2);
    //    vertices.Add(p3);

    //    colors.Add(colorStart);
    //    colors.Add(colorEnd);
    //    colors.Add(colorEnd);
    //    colors.Add(colorStart);

    //    normals.Add(Vector3.up);
    //    normals.Add(Vector3.up);
    //    normals.Add(Vector3.up);
    //    normals.Add(Vector3.up);

    //    triangles.Add(index + 0);
    //    triangles.Add(index + 1);
    //    triangles.Add(index + 2);

    //    triangles.Add(index + 2);
    //    triangles.Add(index + 3);
    //    triangles.Add(index + 0);
    //}
}
