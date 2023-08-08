using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TurretAuhoring : MonoBehaviour
{

}

public class TurretBaker : Baker<TurretAuhoring>
{
    public override void Bake(TurretAuhoring authoring)
    {
        GetEntity(TransformUsageFlags.Dynamic);
    }
}
