using Unity.Burst;
using Unity.Entities;

using Game.Merge.Groups;

namespace Game.Merge.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(MergeGroup))]
    public partial struct MergeApplySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}

