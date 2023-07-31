using UnityEngine;

namespace Warships
{
    /// <summary>
    /// Container for shell stats
    /// </summary>
    [CreateAssetMenu(menuName = "Warships/ShellData")]
    public class CannonAmmoData : ScriptableObject
    {
        public enum ProjectileType
        {
            ARMOR_PIERCING, HIGH_EXPLOSIVE
        }

        [Tooltip("Shell type")]
        public ProjectileType Type;
        [Tooltip("Projectile speed in units per second")]
        public float ProjectileSpeed;
        [Tooltip("Damage per shot")]
        public int Damage;
        [Tooltip("Projectile prefab")]
        public GameObject Projectile;
        [Tooltip("Shell penetration in millimeters at point blank range")]
        public float Penetration;
        [Tooltip("Graph of shell penetration at point blank to maximum range")]
        public AnimationCurve PenetrationOverDistance;

        public float scatterAngleMin = 0.001f;
        public float scatterAngleMax = 1f;
    }
}
