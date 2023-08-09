using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UpdateAfter(typeof(GridGroup))]
    public partial class MergeGroup : Component​System​Group
    {

    }
}
