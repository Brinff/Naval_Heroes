using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionMetalSystem : ProjectileExplosionSystem<SurfaceMetalTag>
{
    protected override void Place(SharedData sharedData, Vector3 position, Vector3 direction)
    {
        sharedData.Get<VFXImpactExplosion>().Play(position, Quaternion.LookRotation(direction));
    }
}
