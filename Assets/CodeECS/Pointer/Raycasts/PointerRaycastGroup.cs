
using Unity.Entities;
using Game.Pointer.Systems;

namespace Game.Pointer.Groups
{
    [UpdateInGroup(typeof(PointerGroup)), UpdateAfter(typeof(PointerInputDataSystem))]
    public partial class PointerRaycastGroup : Component​System​Group
    {

    }
}
