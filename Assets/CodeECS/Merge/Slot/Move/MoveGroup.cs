
using Unity.Entities;

namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(MergeGroup))]
    public partial class MoveGroup : ComponentSystemGroup
    {

    }
}
