using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct GridParent : IComponentData, IEnableableComponent
{
    public Entity value;
}
