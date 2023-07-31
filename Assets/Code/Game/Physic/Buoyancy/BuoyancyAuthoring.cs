using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using OceanSystem;
using Leopotam.EcsLite;


public struct BuoyancyComponent
{
    public BuoyancyAuthoring buoyancyController;
}

public class BuoyancyAuthoring : MonoBehaviour, IEntityAuthoring
{
    public float maxAngularVelocity = 0.05f;
    public Vector3 dragByAxis = new Vector3(1, 1, 0.1f);

    //private OceanSimulation m_OceanSimulation;
    private BuoyancyPoint[] m_BuoyancyPoints;

    [SerializeField]
    private Rigidbody m_Rididbody = null;
    [SerializeField]
    private Collider m_Collider;


    private Vector3[] heights; // water height array offset to water levels
    private Vector3[] normals; // water normal array 
    private Vector3[] samplePoints; // sample points for height calc
    private int _guid; // GUID for the height system

    //private void OnEnable()
    //{
    //    m_OceanSimulation = WorldManager.Instance.oceanSimulation;
    //}

    public bool isEnable => gameObject.activeInHierarchy;

    private void OnDrawGizmos()
    {
        //using(new GizmosScope())
        //{
        //    if (m_Collider != null)
        //    {
                
        //    }
        //}
    }

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var buoyancyComponent = ref ecsWorld.GetPool<BuoyancyComponent>().Add(entity);
        buoyancyComponent.buoyancyController = this;

        m_BuoyancyPoints = m_Rididbody.GetComponentsInChildren<BuoyancyPoint>();

        heights = new Vector3[m_BuoyancyPoints.Length];// new NativeSlice<float3>();
        normals = new Vector3[m_BuoyancyPoints.Length];//new NativeSlice<float3>();
        samplePoints = new Vector3[m_BuoyancyPoints.Length];

        m_Rididbody.isKinematic = false;
        m_Rididbody.useGravity = true;
    }

    //public void Start()
    //{



    //    //_guid = GerstnerWavesJobs.GenId();

    //}

    [SerializeField, Range(0, 1)]
    private float m_Buoyancy = 1;

    public void SetBuoyancy(float buoyancy)
    {
        m_Buoyancy = buoyancy;
        foreach (var item in m_BuoyancyPoints)
        {
            item.SetBuoyancy(m_Buoyancy);
        }
    }

    public void RunUpdate(OceanSimulation oceanSimulation)
    {
        int count = m_BuoyancyPoints.Length;

        for (int i = 0; i < count; i++)
        {
            BuoyancyPoint buoyancy = m_BuoyancyPoints[i];
            if (buoyancy == null) continue;
            if (!buoyancy.enabled) continue;

            samplePoints[i] = buoyancy.transform.position;
        }

        if (oceanSimulation.Collision != null)
        {
            for (int i = 0; i < heights.Length; i++)
            {
                if (oceanSimulation.Collision.GetWaterData(samplePoints[i], out Vector3 waterPosition, out Vector3 waterNormal))
                {
                    heights[i] = waterPosition;
                    normals[i] = waterNormal;
                }
            }
        }
    }

    public void RunFixedUpdate()
    {
        Vector3 force = Vector3.zero;
        Vector3 torque = Vector3.zero;

        int count = m_BuoyancyPoints.Length;

        if (count == 0)
        {
            m_Rididbody.Sleep();
            return;
        }

        for (int i = 0; i < count; i++)
        {
            BuoyancyPoint buoyancy = m_BuoyancyPoints[i];
            if (buoyancy == null) continue;
            if (!buoyancy.enabled) continue;

            buoyancy.UpdateForces(heights[i] + new Vector3(samplePoints[i].x, 0, samplePoints[i].z), normals[i], m_Rididbody, count);

            force += buoyancy.force;
            torque += buoyancy.torque;
        }

        //float dot = Vector3.Dot(transform.right, body.velocity);
        //body.AddRelativeTorque(new Vector3(0, 0, -dot), ForceMode.Acceleration);
        //Debug.Log(dot);
        m_Rididbody.maxAngularVelocity = maxAngularVelocity;
        m_Rididbody.AddForce(force, ForceMode.Force);
        m_Rididbody.AddTorque(torque, ForceMode.Force);

        Vector3 velocity = m_Rididbody.transform.InverseTransformVector(m_Rididbody.velocity);
        Vector3 drag = Vector3.one - Time.fixedDeltaTime * dragByAxis;
        velocity = Vector3.Scale(velocity, drag);
        m_Rididbody.velocity = m_Rididbody.transform.TransformVector(velocity);

    }

}