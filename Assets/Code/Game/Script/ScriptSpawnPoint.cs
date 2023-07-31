using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptSpawnPoint : MonoBehaviour
{
    protected virtual Color GetColor()
    {
        return Color.white;
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Color color = GetColor();
        using (new Handles.DrawingScope(color, transform.localToWorldMatrix))
        {
            Handles.ArrowHandleCap(0, new Vector3(0, 0, 40), Quaternion.LookRotation(Vector3.forward), 10, EventType.Repaint);
        }

        using (new GizmosScope(transform.localToWorldMatrix, color))
        {
            Gizmos.DrawLine(new Vector3(-10, 0, 0), new Vector3(0, 0, 10));
            Gizmos.DrawLine(new Vector3(10, 0, 0), new Vector3(0, 0, 10));
            GizmosUtility.DrawCircle(Vector3.zero, Quaternion.LookRotation(Vector3.up), new Vector2(10, 40));
        }
    }

#endif
}
