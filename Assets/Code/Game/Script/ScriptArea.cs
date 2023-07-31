using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptArea : MonoBehaviour
{
    [SerializeField]
    private float m_Radius;
    private void OnDrawGizmos()
    {
        GizmosUtility.DrawCircle(transform.position, Quaternion.LookRotation(Vector3.up), m_Radius);
    }
}
