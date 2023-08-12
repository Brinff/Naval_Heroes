
using Game.Merge.Systems;
using Unity.Entities;

namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(ItemGroup), OrderLast = true)]
    public partial class EndItemGroup : ComponentSystemGroup
    {

    }
}
