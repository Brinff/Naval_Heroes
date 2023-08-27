using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : IProjectile
{
    public float velocity;
    public float timeFactor = 1;
    public float damage;
    public EcsPackedEntity owner;
    public float scatterAngleMin;
    public float scatterAngleMax;
    public int team;


    public void Launch(EcsWorld world, Vector3 position, Vector3 direction)
    {
        var projectile = world.NewEntity();

        ref ProjectileTransform projectileTransform = ref world.GetPool<ProjectileTransform>().Add(projectile);
        projectileTransform.position = position;
        projectileTransform.rotation = Quaternion.LookRotation(direction);

        ref Team team = ref world.GetPool<Team>().Add(projectile);
        team.id = this.team;

        ref ProjectilePhysic projectilePhysic = ref world.GetPool<ProjectilePhysic>().Add(projectile);
        float angleX = Random.Range(-1f, 1f) * Random.Range(scatterAngleMin, scatterAngleMax);
        float angleY = Random.Range(-1f, 1f) * Random.Range(scatterAngleMin, scatterAngleMax);
        projectilePhysic.velocity = Quaternion.LookRotation(direction) * Quaternion.Euler(angleX, angleY, 0) * Vector3.forward * velocity;
        projectilePhysic.drag = 0;
        projectilePhysic.timeFactor = timeFactor;

        world.GetPool<ProjectileRenderer>().Add(projectile);
        world.GetPool<ProjectileTrailRenderer>().Add(projectile);

        world.GetPool<ProjectileWaterCollision>().Add(projectile);

        ref var projectileRaycast = ref world.GetPool<ProjectileRaycast>().Add(projectile);
        projectileRaycast.previusPosition = position;

        ref ProjectileDamage projectileDamage = ref world.GetPool<ProjectileDamage>().Add(projectile);
        projectileDamage.damage = damage;

        ref ProjectileOwnerComponent projectileOwnerComponent = ref world.GetPool<ProjectileOwnerComponent>().Add(projectile);

        world.GetPool<ProjectileExplosionComponent>().Add(projectile);

        projectileOwnerComponent.entity = owner;
    }
}
