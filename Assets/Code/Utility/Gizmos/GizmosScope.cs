using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public struct GizmosScope : System.IDisposable
{
    private Matrix4x4 m_Matrix;
    private Color m_Color;
    public GizmosScope(Matrix4x4 matrix, Color color)
    {
        m_Matrix = Gizmos.matrix;
        m_Color = Gizmos.color;
        Gizmos.matrix = matrix;
        Gizmos.color = color;
    }

    public GizmosScope(Matrix4x4 matrix) : this(matrix, Gizmos.color)
    {
        
    }

    public GizmosScope(Color color) : this(Gizmos.matrix, color)
    {

    }

    public void Dispose()
    {
        Gizmos.matrix = m_Matrix;
        Gizmos.color = m_Color;
    }


}
