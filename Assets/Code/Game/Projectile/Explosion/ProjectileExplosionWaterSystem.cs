using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionWaterSystem : ProjectileExplosionSystem<SurfaceWaterTag>
{
    protected override void Place(PoolSystem pools, Vector3 position, Vector3 direction)
    {
        pools.GetPool<VFXWaterSplash>().Play(position + Vector3.up, Quaternion.FromToRotation(Vector3.up, -direction));
    }
}
