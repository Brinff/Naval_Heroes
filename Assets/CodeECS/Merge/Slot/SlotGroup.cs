using Game.Merge.Groups;
using Unity.Entities;
namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(MergeGroup), OrderFirst = true)]
    public partial class SlotGroup : ComponentSystemGroup
    {

    }
}
