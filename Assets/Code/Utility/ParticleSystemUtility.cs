using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleSystemUtility
{
    public static void SetEmission(this ParticleSystem particleSystem, bool enabled, bool includeChild = false)
    {
        if (includeChild)
        {
            var particleSystems = particleSystem.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particleSystems.Length; i++)
            {
                var e = particleSystems[i].emission;
                e.enabled = enabled;
            }
        }
        else
        {
            var e = particleSystem.emission;
            e.enabled = enabled;
        }
    }
}
