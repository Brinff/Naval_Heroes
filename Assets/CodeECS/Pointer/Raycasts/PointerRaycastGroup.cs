using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerInputDataSystem))]
public partial class PointerRaycastGroup : Component​System​Group
{

}
