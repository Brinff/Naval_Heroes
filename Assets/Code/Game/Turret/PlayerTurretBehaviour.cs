using System.Collections;
using UnityEngine;

//public class PlayerTurretBehaviour : ITurretAimBehaviour, ITurretFireBehaviour
//{
//    private EyeView m_Eye;
//    private float m_MaxRange;
//    private Plane m_Plane;

//    public PlayerTurretBehaviour(EyeView eye, float maxRange)
//    {
//        m_Eye = eye;
//        m_Plane = new Plane(Vector3.up, 0);
//        m_MaxRange = maxRange;
//    }

//    public Vector3 GetAimPoint(Turret turret)
//    {
//        Ray ray = m_Eye.GetRay();
//        if (m_Plane.Raycast(ray, out float d))
//        {
//            d = Mathf.Clamp(d, 100, m_MaxRange);
//            return ray.GetPoint(d);
//        }
//        return ray.GetPoint(m_MaxRange);
//    }

//    private bool m_IsFire = false;

//    public void SetFire(bool isFire)
//    {
//        m_IsFire = isFire;
//    }

//    public bool GetFire(Turret turret)
//    {
//        return m_IsFire;
//    }
//}