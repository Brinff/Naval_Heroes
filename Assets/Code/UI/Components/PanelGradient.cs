using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

[AddComponentMenu("UI/Gradient")]
public class PanelGradient : BaseMeshEffect
{
    [SerializeField]
    private Color[] m_Colors;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;


        var output = ListPool<UIVertex>.Get();
        vh.GetUIVertexStream(output);

        for (int i = 0; i < Mathf.Min(m_Colors.Length, output.Count); i++)
        {
            var vertex = output[i]; 
            vertex.color *= m_Colors[i];
            output[i] = vertex;
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(output);
        ListPool<UIVertex>.Release(output);
    }
}
