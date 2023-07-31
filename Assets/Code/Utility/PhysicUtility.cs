
using UnityEngine;

public static class PhysicUtility
{
    public static float CalculateLinearForce(Rigidbody rigidbody, float liniarForce)
    {
        return liniarForce * rigidbody.mass / (1 / rigidbody.drag - Time.fixedDeltaTime);
    }

    public static float CalculateAngularForce(Rigidbody rigidbody, float liniarForce)
    {
        return liniarForce * rigidbody.mass / (1 / rigidbody.angularDrag - Time.fixedDeltaTime);
    }
}