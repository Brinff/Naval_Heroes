using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BuoyancyPoint : MonoBehaviour
{
    public float radius = 0.3f;

    //[Range(0.0f, 10.0f)]
    public float slope = 5f;

    private Vector3 outpos;

    private Vector3 normal;

    [NonSerialized]
    public Vector3 force;

    [NonSerialized]
    public Vector3 torque;


    [SerializeField, Range(0f, 1f)]
    private float m_Buoyancy = 1;

    public void SetBuoyancy(float buoyancy)
    {
        m_Buoyancy = buoyancy;
    }

    public void UpdateForces(Vector3 waterPosition, Vector3 waterNormal, Rigidbody body, int count)
    {
        Vector3 pos = transform.position - Vector3.up * radius;
        outpos = waterPosition;

        if (slope > 0f)
        {
            normal = waterNormal;
        }

        Vector3 delta = outpos - pos;

        if (delta.y > 0f)
        {
            float forceConst = body.mass / (1 / 1 - Time.fixedDeltaTime);
            Vector3 targetForce = -Physics.gravity * delta.y * forceConst / count;
            force = targetForce * m_Buoyancy;
        }

        Vector3 r = pos - body.worldCenterOfMass;
        torque = Vector3.Cross(r, force);

        if (slope > 0f)
        {
            torque += Vector3.Cross(body.transform.up, normal) * slope;
        }
    }

    void OnDrawGizmos()
    {
        if (!enabled) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

