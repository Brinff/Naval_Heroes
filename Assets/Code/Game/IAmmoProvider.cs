using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    public void Launch(EcsWorld world, Vector3 position, Vector3 direction);
}

public interface IAmmo
{
    public IProjectile GetProjectile();
}

public interface IAmmoProvider
{
    public IAmmo GetAmmo();
}
