using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionMetalSystem : ProjectileExplosionSystem<SurfaceMetalTag>
{
    protected override void Place(PoolSystem pools, Vector3 position, Vector3 direction)
    {
        pools.GetPool<VFXImpactExplosion>().Play(position, Quaternion.LookRotation(direction));
    }
}
