using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFit : MonoBehaviour
{
    public Vector3 size;

    public TestFit parent;

    public float GetRatio()
    {
        return size.x / size.z;
    }

    private void OnDrawGizmos()
    {
        using (new GizmosScope(transform.localToWorldMatrix))
        {

            Gizmos.DrawWireCube(Vector3.zero, size);
            if (parent)
            {
                float ratio = GetRatio();
                float parentRatio = 1;

                if (parent.size.x > parent.size.z) parentRatio = 1 * parent.GetRatio();
                else parentRatio = 1 / parent.GetRatio();

                if (size.x > size.z) transform.localScale = parentRatio * Vector3.one / ratio;
                else transform.localScale = parentRatio * Vector3.one;
            }
        }
    }
}
