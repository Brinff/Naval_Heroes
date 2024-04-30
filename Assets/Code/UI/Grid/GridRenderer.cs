using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Grid
{
    public class GridRenderer : Graphic
    {
        public float stroke = 0;
        public bool isFrame;
        public float width = 1f;
        public float tails = 0;

        public Vector2 scale;
        public Vector2 center;
        
        [SerializeField]
        private List<GridRect> m_GridRects;

        public void AddGridRect(GridRect gridRect)
        {
            m_GridRects.Add(gridRect);
            SetVerticesDirty();
        }

        public void RemoveGridRect(GridRect gridRect)
        {
            m_GridRects.Remove(gridRect);
            SetVerticesDirty();
        }

        public void Clear()
        {
            m_GridRects.Clear();
            SetVerticesDirty();
        }

        private void Update()
        {
            bool isDirty = false;
            foreach (var gridRect in m_GridRects)
            {
                if (gridRect.isDirty)
                {
                    isDirty = true;
                }
                gridRect.isDirty = false;
            }
            
            if(isDirty) SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            foreach (var gridRect in m_GridRects)
            {
                AddRect(gridRect.position, gridRect.size, vh);
            }
        }
        
        private void AddRect(Vector2 position, Vector2 size, VertexHelper vh)
        {
            Vector2 orgin = new Vector2(position.x * scale.x, position.y * scale.y);
            Vector2 halfScale = new Vector2(scale.x, scale.y) * 0.5f;
            Color32 colorStart = color;
            Color32 colorEnd = color;
            colorEnd.a = 0;
            
            for (int x = 0; x < size.x + 1; x++)
            {
                Vector2 originA = orgin + new Vector2(x * scale.x, 0) + new Vector2(center.x, center.y - width) - halfScale;

                float sizeY = size.y * scale.y + width * 2;
                if (isFrame)
                {
                    if (x == 0 || x == size.x)
                    {
                        AddLine(originA, Vector2.up, sizeY, colorStart, colorStart, stroke, width, vh);

                        if (tails > 0)
                        {
                            Vector2 originB = originA + Vector2.up * sizeY;
                            AddLine(originA, -Vector2.up, tails, colorStart, colorEnd, 0, width, vh);
                            AddLine(originB, Vector2.up, tails, colorStart, colorEnd, 0, width, vh);
                        }
                    }
                }
                else
                {
                    AddLine(originA, Vector2.up, sizeY, colorStart, colorStart, 0, width, vh);

                    if (tails > 0)
                    {
                        Vector2 originB = originA + Vector2.up * sizeY;
                        AddLine(originA, -Vector2.up, tails, colorStart, colorEnd, 0, width, vh);
                        AddLine(originB, Vector2.up, tails, colorStart, colorEnd, 0, width, vh);
                    }
                }

            }

            for (int y = 0; y < size.y + 1; y++)
            {
                Vector2 originA = orgin + new Vector2(0, y * scale.y) + new Vector2(center.x - width, center.y) - halfScale;
                float sizeX = size.x * scale.x + width * 2;
                if (isFrame)
                {
                    if (y == 0 || y == size.y)
                    {
                        AddLine(originA, Vector2.right, sizeX, colorStart, colorStart, stroke, width, vh);

                        if (tails > 0)
                        {
                            Vector2 originB = originA + Vector2.right * sizeX;
                            AddLine(originA, -Vector2.right, tails, colorStart, colorEnd, 0, width, vh);
                            AddLine(originB, Vector2.right, tails, colorStart, colorEnd, 0, width, vh);
                        }
                    }
                }
                else
                {
                    AddLine(originA, Vector2.right, sizeX, colorStart, colorStart, 0, width, vh);

                    if (tails > 0)
                    {
                        Vector2 originB = originA + Vector2.right * sizeX;
                        AddLine(originA, -Vector2.right, tails, colorStart, colorEnd, 0, width, vh);
                        AddLine(originB, Vector2.right, tails, colorStart, colorEnd, 0, width, vh);
                    }
                }
            }
        }
        private void AddLine(Vector2 start, Vector2 direction, float size, Color colorStart, Color colorEnd, float stroke, float width, VertexHelper vh)
        {
            AddLine(start, start + direction * size, colorStart, colorEnd, stroke, width, vh);
        }

        private void AddLine(Vector2 start, Vector2 end, Color colorStart, Color colorEnd, float stroke, float width, VertexHelper vh)
        {
            
            Vector2 direction = end - start;
            direction.Normalize();


            Vector2 right = Vector2.Perpendicular(direction);


            if (stroke > 0)
            {
                float distance = Vector2.Distance(start, end);
                int countStroke = Mathf.RoundToInt(distance / stroke);
                float lenght = distance / countStroke;


                Vector2 p0 = start + right * width;
                Vector2 p1 = end + right * width;
                Vector2 p2 = end - right * width;
                Vector2 p3 = start - right * width;

                for (int i = 0; i < countStroke; i++)
                {
                    if (i % 2 == 0) continue;

                    Vector2 localStart = Vector2.Lerp(start, end, (float)i / countStroke);
                    Vector2 localEnd = localStart + direction * lenght;
                    p0 = localStart + right * width;
                    p1 = localEnd + right * width;
                    p2 = localEnd - right * width;
                    p3 = localStart - right * width;

                    int index = vh.currentVertCount;
                    
                    vh.AddVert(p0, colorStart, Vector4.zero);
                    vh.AddVert(p1, colorEnd, Vector4.zero);
                    vh.AddVert(p2, colorEnd, Vector4.zero);
                    vh.AddVert(p3, colorStart, Vector4.zero);
                    
                    vh.AddTriangle(index + 0, index + 1, index + 2);
                    vh.AddTriangle(index + 2, index + 3, index + 0);
                    /*triangles.Add(index + 0);
                    triangles.Add(index + 1);
                    triangles.Add(index + 2);

                    triangles.Add(index + 2);
                    triangles.Add(index + 3);
                    triangles.Add(index + 0);*/
                    
                    /*vertices.Add(p0);
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
                    normals.Add(Vector3.up);*/


                }
            }
            else
            {
                int index = vh.currentVertCount;

                Vector3 p0 = start + right * width;
                Vector3 p1 = end + right * width;
                Vector3 p2 = end - right * width;
                Vector3 p3 = start - right * width;

                vh.AddVert(p0, colorStart, Vector4.zero);
                vh.AddVert(p1, colorEnd, Vector4.zero);
                vh.AddVert(p2, colorEnd, Vector4.zero);
                vh.AddVert(p3, colorStart, Vector4.zero);
                    
                vh.AddTriangle(index + 0, index + 1, index + 2);
                vh.AddTriangle(index + 2, index + 3, index + 0);
                
                /*vertices.Add(p0);
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
                triangles.Add(index + 0);*/
            }
        }
    }
}