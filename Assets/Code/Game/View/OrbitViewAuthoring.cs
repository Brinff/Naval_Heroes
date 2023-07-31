using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class OrbitViewAuthoring : MonoBehaviour, IEntityAuthoring
{
    [SerializeField]
    private Transform m_Transform;
    [SerializeField]
    private Vector2 m_FocalEllepse = new Vector2(20, 40);
    [SerializeField]
    private Vector2 m_FocalCenter = new Vector2(0, 0);
    [SerializeField]
    private Vector2 m_OrbitElllepse = new Vector2(100, 100);
    [SerializeField]
    private float m_Height;
    public bool isEnable => gameObject.activeInHierarchy;

    public Matrix4x4 GetOrbitMatrix()
    {
        Transform tr = m_Transform ? m_Transform : transform;
        Vector3 forward = Vector3.Normalize(Vector3.Cross(tr.right, Vector3.up));
        Matrix4x4 matrix = Matrix4x4.TRS(tr.position, Quaternion.LookRotation(forward) * Quaternion.Euler(90, 0, 0), Vector3.one);
        return matrix;
    }

    //public Vector3 GetOrbit(Quaternion rotation)
    //{
    //    Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix();
    //    Matrix4x4 worldToOrbitMatrix = Matrix4x4.Inverse(orbitToWorldMatrix);

    //    Quaternion rotationY = QuaternionUtility.ProjectOnPlane(rotation, Quaternion.identity, Vector3.up);

    //    Vector2 directionTarget = worldToOrbitMatrix.MultiplyVector(rotationY * Vector3.forward);
    //    //Vector2 directionTarget = Vector3.Normalize(targetPosition);

    //    float2x2 d = new float2x2(new float2(m_FocalEllepse.x * m_FocalEllepse.x, 0), new float2(0, m_FocalEllepse.y * m_FocalEllepse.y));
    //    float2x1 n = new float2x1(math.normalize(new float2(-directionTarget.x, -directionTarget.y)));

    //    var dm = mathMatrix.mul(d, n);
    //    float2 p = dm.c0 / math.sqrt(mathMatrix.mul(mathMatrix.mul(mathMatrix.transpose(n), d), n));

    //    //Vector2 directionView = Vector3.Normalize(-directionTarget);

    //    //var orbitDistance = geometry.rayIntersectEllipse(p, -(float2)directionView, m_OrbitElllepse);
    //    //Vector2 cameraPoint = (Vector2)p - directionView * orbitDistance;

    //    return orbitToWorldMatrix.MultiplyPoint(new Vector2(p.x,p.y));
    //    //Vector3 varticalLookAt = Vector3.Normalize(worldTarget - position);
    //    //Vector3 horizontalLookAt = orbitToWorldMatrix.MultiplyVector(directionView);
    //    //rotation = Quaternion.LookRotation(Vector3.Normalize(new Vector3(horizontalLookAt.x, varticalLookAt.y, horizontalLookAt.z)));
    //}

    public void GetOrbitRP(Vector3 worldTarget, out Vector3 position, out Quaternion rotation)
    {
        Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix();
        Matrix4x4 worldToOrbitMatrix = Matrix4x4.Inverse(orbitToWorldMatrix);

        Vector2 targetPosition = worldToOrbitMatrix.MultiplyPoint(worldTarget);
        Vector2 directionTarget = Vector3.Normalize(targetPosition - m_FocalCenter);

        float2x2 d = new float2x2(new float2(m_FocalEllepse.x * m_FocalEllepse.x, 0), new float2(0, m_FocalEllepse.y * m_FocalEllepse.y));
        float2x1 n = new float2x1(math.normalize(new float2(-directionTarget.x, -directionTarget.y)));

        var dm = mathMatrix.mul(d, n);
        float2 p = dm.c0 / math.sqrt(mathMatrix.mul(mathMatrix.mul(mathMatrix.transpose(n), d), n));

        Vector2 directionView = Vector3.Normalize(targetPosition - (Vector2)p);

        var orbitDistance = geometry.rayIntersectEllipse(p, -(float2)directionView, m_OrbitElllepse);
        Vector2 cameraPoint = (Vector2)p - directionView * orbitDistance;

        position = orbitToWorldMatrix.MultiplyPoint(cameraPoint) + Vector3.up * m_Height;
        Vector3 varticalLookAt = Vector3.Normalize(worldTarget - position);
        Vector3 horizontalLookAt = orbitToWorldMatrix.MultiplyVector(directionView);
        rotation = Quaternion.LookRotation(Vector3.Normalize(new Vector3(horizontalLookAt.x, varticalLookAt.y, horizontalLookAt.z)));
    }

    private void OnDrawGizmosSelected()
    {
        Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix();

        using (new GizmosScope(orbitToWorldMatrix))
        {
            Gizmos.color = Color.blue;

            //Gizmos.DrawSphere(m_FocalCenter, 1);

            Gizmos.color = Color.yellow;
            GizmosUtility.DrawCircle(Vector3.zero, Quaternion.identity, m_FocalEllepse);

            Gizmos.color = Color.red;
            GizmosUtility.DrawCircle(Vector3.zero, Quaternion.identity, m_OrbitElllepse);
        }
    }

    public void Bake(int entity, EcsWorld ecsWorld)
    {
        ref var orbitView = ref ecsWorld.GetPool<OrbitViewComponent>().Add(entity);
        orbitView.transform = m_Transform ? m_Transform : this.transform;
        orbitView.focalEllepse = m_FocalEllepse;
        orbitView.focalCenter = m_FocalCenter;
        orbitView.orbitElllepse = m_OrbitElllepse;
        orbitView.height = m_Height;
    }
}
