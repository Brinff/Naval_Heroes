using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class ShipOrbitView : MonoBehaviour, IViewPosition, IViewRotation, IViewTarget, IViewPerform, IViewFieldOfView, IViewRotationSensitivity
{
    [SerializeField, Range(5, 179)]
    private float m_FieldOfView = 60;
    [SerializeField]
    private Vector2 m_RotationSensitivity = new Vector2(1, 1);

    public Vector2 focalEllepse = new Vector2(20, 40);
    public Vector2 focalCenter = new Vector2(0, 0);
    public Vector2 focalAxis = new Vector2(1, 0);



    [Range(0, 1)]
    public float focalForward;
    [Range(0, 1)]
    public float focalBackward;

    public Vector2 orbitElllepse = new Vector2(100, 100);


    public float height;


    private Transform m_Target;

    public Vector3 position => m_Position;

    public Quaternion rotation => m_Rotation;

    public float fieldOfView => m_FieldOfView;

    public Vector2 rotationSensitivity => m_RotationSensitivity;

    private Vector3 m_Position = Vector3.zero;
    private Quaternion m_Rotation = Quaternion.identity;


    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    public Matrix4x4 GetOrbitMatrix()
    {
        Vector3 forward = Vector3.Normalize(Vector3.Cross(transform.right, Vector3.up));
        Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(forward) * Quaternion.Euler(90, 0, 0), Vector3.one);
        return matrix;
    }

    public void Perform(float deltaTime)
    {
        if (m_Target != null)
            GetOrbitRP(m_Target.position, out m_Position, out m_Rotation);
    }

    public void GetOrbitRP(Vector3 worldTarget, out Vector3 position, out Quaternion rotation)
    {
        Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix();
        Matrix4x4 worldToOrbitMatrix = Matrix4x4.Inverse(orbitToWorldMatrix);


        Vector2 targetPosition = worldToOrbitMatrix.MultiplyPoint(worldTarget);



        Vector2 directionTarget = Vector3.Normalize(targetPosition - focalCenter);

        float2x2 d = new float2x2(new float2(focalEllepse.x * focalEllepse.x, 0), new float2(0, focalEllepse.y * focalEllepse.y));
        float2x1 n = new float2x1(math.normalize(new float2(-directionTarget.x, -directionTarget.y)));

        var dm = mathMatrix.mul(d, n);
        float2 p = dm.c0 / math.sqrt(mathMatrix.mul(mathMatrix.mul(mathMatrix.transpose(n), d), n));

        Vector2 directionView = Vector3.Normalize(targetPosition - (Vector2)p);

        var orbitDistance = geometry.rayIntersectEllipse(p, -(float2)directionView, orbitElllepse);
        Vector2 cameraPoint = (Vector2)p - directionView * orbitDistance;

        position = orbitToWorldMatrix.MultiplyPoint(cameraPoint) + Vector3.up * height;
        Vector3 varticalLookAt = Vector3.Normalize(worldTarget - position);
        Vector3 horizontalLookAt = orbitToWorldMatrix.MultiplyVector(directionView);
        rotation = Quaternion.LookRotation(Vector3.Normalize(new Vector3(horizontalLookAt.x,varticalLookAt.y, horizontalLookAt.z)));
    }

    private void OnDrawGizmos()
    {

        Matrix4x4 orbitToWorldMatrix = GetOrbitMatrix();

        Vector2 focalForwardPoint = Vector2.Scale(focalAxis, focalEllepse) * focalForward;
        Vector2 focalBackwardPoint = Vector2.Scale(-focalAxis, focalEllepse) * focalBackward;

        //Vector2 projectPoint = geometry.projectPointAtLine(targetPosition, focalForwardPoint, focalBackwardPoint);

        //Matrix4x4 worldToOrbitMatrix = Matrix4x4.Inverse(orbitToWorldMatrix);


        //Vector2 targetPosition = worldToOrbitMatrix.MultiplyPoint(target.position);

        //Vector2 focalForwardPoint = Vector2.Scale(focalAxis, focalEllepse) * focalForward;
        //Vector2 focalBackwardPoint = Vector2.Scale(-focalAxis, focalEllepse) * focalBackward;

        //Vector2 projectPoint = geometry.projectPointAtLine(-targetPosition, focalForwardPoint, focalBackwardPoint);

        //Vector2 directionTarget = Vector3.Normalize(targetPosition - projectPoint); 

        //float2x2 d = new float2x2(new float2(focalEllepse.x * focalEllepse.x, 0), new float2(0, focalEllepse.y * focalEllepse.y));
        //float2x1 n = new float2x1(math.normalize(new float2(-directionTarget.x, -directionTarget.y)));

        //var dm = mathMatrix.mul(d, n);
        //float2 p = dm.c0 / math.sqrt(mathMatrix.mul(mathMatrix.mul(mathMatrix.transpose(n), d), n));

        //Vector2 directionView = Vector3.Normalize(targetPosition - (Vector2)p);

        //var orbitDistance = geometry.rayIntersectEllipse(p, -(float2)directionView, orbitElllepse);
        //Vector2 cameraPoint = (Vector2)p - directionView * orbitDistance;
        //GetOrbitRP(target.position, out Vector3 position, out Quaternion rotation);
        //viewOrgin.transform.position = position;
        //viewOrgin.transform.rotation = rotation;

        using (new GizmosScope(orbitToWorldMatrix))
        {

            //Gizmos.DrawLine((Vector2)p, (Vector2)p - directionView * orbitDistance);
            //Gizmos.DrawSphere(cameraPoint, 1);

            //Gizmos.DrawCube(projectPoint, Vector3.one * 1);

            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(focalCenter, 1);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(focalForwardPoint, focalBackwardPoint);
            GizmosUtility.DrawCircle(Vector3.zero, Quaternion.identity, focalEllepse);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(focalForwardPoint, 1);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(focalBackwardPoint, 1);

            Gizmos.color = Color.red;
            GizmosUtility.DrawCircle(Vector3.zero, Quaternion.identity, orbitElllepse);
        }
    }
}
