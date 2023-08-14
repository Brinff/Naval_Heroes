
using Game.Merge.Systems;
using Unity.Entities;

namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(MergeGroup)), UpdateAfter(typeof(MergeApplySystem))]
    public partial class ItemGroup : ComponentSystemGroup
    {

    }
}
