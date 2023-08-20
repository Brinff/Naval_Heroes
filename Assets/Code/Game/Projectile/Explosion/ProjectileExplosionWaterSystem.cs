using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionWaterSystem : ProjectileExplosionSystem<SurfaceWaterTag>
{
    protected override void Place(PoolSystem pools, Vector3 position, Vector3 direction)
    {
        pools.GetPool<VFXWaterSplash>().Play(position, Quaternion.FromToRotation(Vector3.up, -direction));
    }
}
