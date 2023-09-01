using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TorpedoProjectile : IProjectile
{
    public float speed;
    public float timeFactor = 1;
    public Vector3 target;
    public float speedLookAt = 1;
    public float damage;
    public EcsPackedEntity owner;
    public int team;


    public void Launch(EcsWorld world, Vector3 position, Vector3 direction)
    {
        var projectile = world.NewEntity();

        ref ProjectileTimerDestroy timer = ref world.GetPool<ProjectileTimerDestroy>().Add(projectile);
        timer.timer = 20;

        ref ProjectileTransform projectileTransform = ref world.GetPool<ProjectileTransform>().Add(projectile);
        projectileTransform.position = position;
        projectileTransform.rotation = Quaternion.LookRotation(direction);

        ref Team team = ref world.GetPool<Team>().Add(projectile);
        team.id = this.team;

        ref ProjectileMoveToTarget moveToTarget = ref world.GetPool<ProjectileMoveToTarget>().Add(projectile);
        //float angleX = Random.Range(-1f, 1f) * Random.Range(scatterAngleMin, scatterAngleMax);
        //float angleY = Random.Range(-1f, 1f) * Random.Range(scatterAngleMin, scatterAngleMax);
        moveToTarget.velocity = direction * speed;
        moveToTarget.speed = speed;
        //projectilePhysic.drag = 0;
        moveToTarget.timeFactor = timeFactor;
        moveToTarget.target = target;
        moveToTarget.speedLookAt = speedLookAt;

        ref var renderer = ref world.GetPool<ProjectileRenderer>().Add(projectile);
        renderer.id = 1;
        ref var trailRenderer = ref world.GetPool<ProjectileTrailRenderer>().Add(projectile);
        trailRenderer.id = 1;

        //world.GetPool<ProjectileWaterCollision>().Add(projectile);

        ref var projectileRaycast = ref world.GetPool<ProjectileRaycast>().Add(projectile);
        projectileRaycast.previusPosition = position;

        ref ProjectileDamage projectileDamage = ref world.GetPool<ProjectileDamage>().Add(projectile);
        projectileDamage.damage = damage;

        ref ProjectileOwnerComponent projectileOwnerComponent = ref world.GetPool<ProjectileOwnerComponent>().Add(projectile);

        world.GetPool<ProjectileExplosionComponent>().Add(projectile);

        projectileOwnerComponent.entity = owner;
    }
}

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
