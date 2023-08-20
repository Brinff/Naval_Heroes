
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game.Grid.Auhoring
{
    [AddComponentMenu("Game/Grid/Grid Renderer")]
    public class GridRendererAuthoring : MonoBehaviour
    {
        public Material material;
        public int layer = 0;
        [ColorUsage(true, true)]
        public Color color;
        [Range(0,1)]
        public float alpha;

        private Material instanceMaterial;

        private void OnEnable()
        {
            var renderer = gameObject.GetOrAddComponent<MeshRenderer>();
            instanceMaterial = Instantiate(material);
            instanceMaterial.renderQueue = 3000 + layer;
            instanceMaterial.SetColor("_Color", color);
            instanceMaterial.SetFloat("_Alpha", alpha);
            renderer.sharedMaterial = instanceMaterial;
        }



        public void SetColor(Color color)
        {
            this.color = color;
            instanceMaterial.SetColor("_Color", color);
        }

        public void SetAlpha(float alpha)
        {
            this.alpha = alpha;
            instanceMaterial.SetFloat("_Alpha", alpha);
        }

        public float width = 1f;
        public float tails = 0;

        private Vector2 scale;
        private Vector2 center;

        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<Color> colors = new List<Color>();
        private List<int> triangles = new List<int>();

        private Mesh mesh;
        private MeshFilter meshFilter;
        public void BeginFill(Vector2 scale, Vector2 center)
        {
            this.scale = scale;
            this.center = center;
            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = $"Grid Mesh";
                mesh.hideFlags = HideFlags.DontSave;
            }

            vertices.Clear();
            normals.Clear();
            colors.Clear();
            triangles.Clear();

            mesh.Clear();
        }

        public void Clear()
        {
            vertices.Clear();
            normals.Clear();
            colors.Clear();
            triangles.Clear();

            mesh.Clear();

            if (meshFilter == null) meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
        }

        public void AddRect(Vector2 position, Vector2 size)
        {
            Vector3 orgin = new Vector3(position.x * scale.x, 0, position.y * scale.y);
            Vector3 halfScale = new Vector3(scale.x, 0, scale.y) * 0.5f;
            for (int x = 0; x < size.x + 1; x++)
            {
                Vector3 originA = orgin + new Vector3(x * scale.x, 0, 0) + new Vector3(center.x, 0, center.y - width) - halfScale;

                float sizeY = size.y * scale.y + width * 2;
                AddLine(originA, Vector3.forward, sizeY, Color.white, Color.white, width);
                if (tails > 0)
                {
                    Vector3 originB = originA + Vector3.forward * sizeY;
                    AddLine(originA, -Vector3.forward, tails, Color.white, Color.clear, width);
                    AddLine(originB, Vector3.forward, tails, Color.white, Color.clear, width);
                }
            }

            for (int y = 0; y < size.y + 1; y++)
            {
                Vector3 originA = orgin + new Vector3(0, 0, y * scale.y) + new Vector3(center.x - width, 0, center.y) - halfScale;
                float sizeX = size.x * scale.x + width * 2;
                AddLine(originA, Vector3.right, sizeX, Color.white, Color.white, width);

                if (tails > 0)
                {
                    Vector3 originB = originA + Vector3.right * sizeX;
                    AddLine(originA, -Vector3.right, tails, Color.white, Color.clear, width);
                    AddLine(originB, Vector3.right, tails, Color.white, Color.clear, width);
                }
            }
        }

        public Mesh EndFill()
        {
            mesh.SetVertices(vertices);
            mesh.SetIndices(triangles, MeshTopology.Triangles, 0);
            mesh.SetNormals(normals);
            mesh.SetColors(colors);
            mesh.RecalculateBounds();

            if (meshFilter == null) meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;

            return mesh;
        }

        private void AddLine(Vector3 start, Vector3 direction, float size, Color colorStart, Color colorEnd, float width)
        {
            AddLine(start, start + direction * size, colorStart, colorEnd, width);
        }

        private void AddLine(Vector3 start, Vector3 end, Color colorStart, Color colorEnd, float width)
        {
            int index = vertices.Count;
            Vector3 direction = Vector3.Normalize(end - start);
            Vector3 right = Vector3.Cross(direction, Vector3.up);
            Vector3 p0 = start + right * width;
            Vector3 p1 = end + right * width;
            Vector3 p2 = end - right * width;
            Vector3 p3 = start - right * width;

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

    //public class GridRendererBaker : Baker<GridRendererAuthoring>
    //{
    //    public override void Bake(GridRendererAuthoring authoring)
    //    {
    //        var entity = GetEntity(authoring, TransformUsageFlags.Renderable);
    //        AddSharedComponentManaged(entity, new GridRenderer() { material = authoring.material });
    //    }
    //}
}
