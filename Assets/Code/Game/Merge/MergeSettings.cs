using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Merge")]
public class MergeSettings : ScriptableObject
{
    [System.Serializable]
    public class MergeGrid
    {

    }

    [System.Serializable]
    public class BattleGrid
    {

    }

    [System.Serializable]
    public class MoveItem
    {
        public float damper;
        public float force;
        public Vector3 rotationByLocalVelocty;
        public float clampRotationByVelocity;
        public float height;
    }

    public MoveItem moveItem;
}
