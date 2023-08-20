using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Merge")]
public class MergeSettings : ScriptableObject
{
    [System.Serializable]
    public class MergeGrid
    {
        [ColorUsage(true, true)]
        public Color active;
        [ColorUsage(true, true)]
        public Color normal;
        [ColorUsage(true, true)]
        public Color reject;
        [ColorUsage(true, true)]
        public Color allow;
    }

    [System.Serializable]
    public class BattleGrid
    {
        [ColorUsage(true, true)]
        public Color field;
        [ColorUsage(true, true)]
        public Color currentPosition;
        [ColorUsage(true, true)]
        public Color position;
        [ColorUsage(true, true)]
        public Color reject;
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
