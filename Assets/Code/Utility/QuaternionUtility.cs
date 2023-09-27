using UnityEngine;
using UnityEngine.Animations;

public static class QuaternionUtility
{
    public static Quaternion ProjectOnPlane(Quaternion rotation, Quaternion projector, Vector3 axis)
    {
        Vector3 forward = rotation * Vector3.forward;
        Vector3 project = Quaternion.Inverse(projector) * Vector3.ProjectOnPlane(forward, projector * axis);
        return Quaternion.LookRotation(project);
    }

    public static Quaternion ClampAngleYX(Quaternion rotation, float minY, float maxY, float minX, float maxX)
    {
        Quaternion rotationY = ProjectOnPlane(rotation, Quaternion.identity, Vector3.up);

        float angleY = Vector3.SignedAngle(Vector3.forward, rotationY * Vector3.forward, Vector3.up);
        angleY = Mathf.Clamp(angleY, -maxY, -minY);
        rotationY = Quaternion.AngleAxis(angleY, Vector3.up);


        Quaternion rotationX = ProjectOnPlane(rotation, rotationY, Vector3.right);
        Vector3 forwardY = rotationX * Vector3.forward;
        Vector3 forwardXZ = Vector3.ProjectOnPlane(forwardY, Vector3.up);
        Vector3 right = rotationX * Vector3.right;
        float angle = Vector3.SignedAngle(forwardXZ, forwardY, right);
        angle = Mathf.Clamp(angle, -maxX, -minX);
        Quaternion clampRotationX = Quaternion.AngleAxis(angle, right);
        return rotationY * clampRotationX;
    }

    public static Quaternion ClampAngleX(Quaternion rotation, float min, float max)
    {
        Quaternion rotationY = ProjectOnPlane(rotation, Quaternion.identity, Vector3.up);
        Quaternion rotationX = ProjectOnPlane(rotation, rotationY, Vector3.right);
        Vector3 forwardY = rotationX * Vector3.forward;
        Vector3 forwardXZ = Vector3.ProjectOnPlane(forwardY, Vector3.up);
        Vector3 right = rotationX * Vector3.right;
        float angle = Vector3.SignedAngle(forwardXZ, forwardY, right);
        angle = Mathf.Clamp(angle, -max, -min);
        Quaternion clampRotationX = Quaternion.AngleAxis(angle, right);
        return rotationY * clampRotationX;
    }

    public static Quaternion ClampAxisAngle(Quaternion rotation, Quaternion parent, float min, float max, Vector3 axis)
    {
        Quaternion newRotation = ProjectOnPlane(rotation, parent, axis);
        Vector3 forward = newRotation * Vector3.forward;
        Vector3 parentForward = parent * Vector3.forward;
        float angle = Vector3.SignedAngle(parentForward, forward, axis);
        angle = Mathf.Clamp(angle, -max, -min);
        return  Quaternion.AngleAxis(angle, axis);
    }


    public static Quaternion AxisAngleSmooth(Quaternion rotation, Quaternion target, Quaternion parent, Vector3 axis, float maxDelat)
    {
        Vector3 parentForward = parent * Vector3.forward;

        Quaternion oldRotation = ProjectOnPlane(rotation, parent, axis);
        Vector3 oldForward = oldRotation * Vector3.forward;

        Quaternion newRotation = ProjectOnPlane(target, parent, axis);
        Vector3 newForward = newRotation * Vector3.forward;


        float newAngle = Vector3.SignedAngle(parentForward, newForward, axis);
        float oldAngle = Vector3.SignedAngle(parentForward, oldForward, axis);

        float angle = Mathf.MoveTowardsAngle(oldAngle, newAngle, maxDelat);

        return Quaternion.AngleAxis(angle, axis);
    }

    public static Quaternion ClampAxisAngleSmooth(Quaternion rotation, Quaternion target, Quaternion parent, float min, float max, Vector3 axis, float maxDelta)
    {
        Vector3 parentForward = parent * Vector3.forward;

        Quaternion oldRotation = ProjectOnPlane(rotation, parent, axis);
        Vector3 oldForward = oldRotation * Vector3.forward;

        Quaternion newRotation = ProjectOnPlane(target, parent, axis);
        Vector3 newForward = newRotation * Vector3.forward;

        float oldAngle = Vector3.SignedAngle(parentForward, oldForward, axis);
        float newAngle = Vector3.SignedAngle(parentForward, newForward, axis);

        newAngle = Mathf.Clamp(newAngle, -max, -min);

        float angle = Mathf.MoveTowards(oldAngle, newAngle, maxDelta);

        angle = Mathf.Clamp(angle, -max, -min);

        return Quaternion.AngleAxis(angle, axis);
    }

    //public static Quaternion ClampRotation(Quaternion rotation, float min, float max, Axis axis)
    //{
    //    Quaternion minRotation = Quaternion.Euler(min, min, min);
    //    Quaternion maxRotation = Quaternion.Euler(max, max, max);
    //    Quaternion finalRotation = rotation;

    //    float rot;
    //    float minAngle;
    //    float maxAngle;

    //    switch (axis)
    //    {
    //        case Axis.X:
    //            rot = Mathf.Abs(finalRotation.x);
    //            minAngle = Mathf.Abs(minRotation.x);
    //            maxAngle = Mathf.Abs(maxRotation.x);
    //            if (rot <= minAngle)
    //            {
    //                finalRotation.x = minRotation.x;
    //            }
    //            if (rot >= maxAngle)
    //            {
    //                finalRotation.x = maxRotation.x;
    //            }
    //            break;
    //        case Axis.Y:
    //            rot = Mathf.Abs(finalRotation.y);
    //            minAngle = Mathf.Abs(minRotation.y);
    //            maxAngle = Mathf.Abs(maxRotation.y);
    //            if (rot <= minAngle)
    //            {
    //                finalRotation.y = minRotation.y;
    //            }
    //            if (rot >= maxAngle)
    //            {
    //                finalRotation.y = maxRotation.y;
    //            }

    //            break;

    //        case Axis.Z:
    //            rot = Mathf.Abs(finalRotation.z);
    //            minAngle = Mathf.Abs(minRotation.z);
    //            maxAngle = Mathf.Abs(maxRotation.z);
    //            if (rot <= minAngle)
    //            {
    //                finalRotation.z = minRotation.z;
    //            }
    //            if (rot >= maxAngle)
    //            {
    //                finalRotation.z = maxRotation.z;
    //            }
    //            break;
    //    }
    //    return finalRotation.normalized;
    //}
}