
using Game.Merge.Systems;
using Unity.Entities;

namespace Game.Merge.Groups
{
    [UpdateInGroup(typeof(ItemGroup), OrderFirst = true)]
    public partial class BeginItemGroup : ComponentSystemGroup
    {

    }
}
