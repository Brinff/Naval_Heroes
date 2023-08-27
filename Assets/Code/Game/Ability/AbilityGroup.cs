using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.EcsLite;

public struct AbilityGroup
{
    public bool isSeparately;
    public int selector;
    public List<EcsPackedEntity> entities;
}
