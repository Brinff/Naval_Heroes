using Unity.Entities;

[UpdateInGroup(typeof(PresentationSystemGroup)), UpdateAfter(typeof(BeginPresentationEntityCommandBufferSystem))]
public partial class GridRenderGroup : PresentationSystemGroup
{

}
