using Game.Merge.Groups;
using Unity.Entities;
namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(ItemGroup))]
    public partial class SlotGroup : ComponentSystemGroup
    {

    }
}
