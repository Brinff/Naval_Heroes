using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionWaterSystem : ProjectileExplosionSystem<SurfaceWaterTag>
{
    protected override void Place(SharedData sharedData, Vector3 position, Vector3 direction)
    {
        sharedData.Get<VFXWaterSplash>().Play(position, Quaternion.FromToRotation(Vector3.up, -direction));
    }
}
