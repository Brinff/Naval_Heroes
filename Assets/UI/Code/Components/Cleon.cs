using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cleon : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_Arrow;
    [SerializeField]
    private float m_Angle;
    /*[SerializeField, Range(0,1)]
    private float m_Position;*/

    [System.Serializable]
    public class Gap
    {
        public float lenght;
        public float value;
    }

    [SerializeField]
    private Gap[] m_Gaps;


    private Gap GetGap(float position)
    {
        position = Mathf.Clamp01(position);
        float start = 0;
        for (int i = 0; i < m_Gaps.Length; i++)
        {
            var gap = m_Gaps[i];
            if(start <= position && position < start + gap.lenght)
            {
                return gap;
            }
            start += gap.lenght;
        }
        return m_Gaps[m_Gaps.Length - 1];
    }

    public float Evaluate(float position)
    {
        position = Mathf.PingPong(position, 1);
        var gap = GetGap(position);
        m_Arrow.localRotation = Quaternion.Euler(0, 0, m_Angle - m_Angle * 2 * position);
        return gap.value;
    }

    private void OnDrawGizmos()
    {
        if (m_Arrow != null)
        {
            m_Arrow.localRotation = Quaternion.Euler(0, 0, m_Angle - m_Angle * 2 * 0);

            using (new GizmosScope(m_Arrow.localToWorldMatrix))
            {
                float start = 0;
                for (int i = 0; i < m_Gaps.Length; i++)
                {
                    var gap = m_Gaps[i];
                    //Vector3 p0 = new Vector3()
                    Quaternion a0 = Quaternion.Euler(0, 0, -m_Angle * 2 * start);
                    Quaternion a1 = Quaternion.Euler(0, 0, -m_Angle * 2 * (start + gap.lenght));
                    Gizmos.DrawLine(Vector3.zero, a0 * Vector3.up * 500);
                    Gizmos.DrawLine(Vector3.zero, a1 * Vector3.up * 500);
                    start += gap.lenght;
                }
            }
        }
    }
}
