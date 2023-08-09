using Unity.Entities;
namespace Game.Data.Groups
{
    [UpdateInGroup(typeof(SimulationSystemGroup)), UpdateAfter(typeof(BeginInitializationEntityCommandBufferSystem))]
    public partial class DataGroup : Component​System​Group
    {

    }
}
