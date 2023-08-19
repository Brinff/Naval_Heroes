using Game.Battle.Events;
using Game.Rendering.Material.Components;
using Unity.Burst;
using Unity.Entities;

namespace Game.Battle.Systems
{
    [BurstCompile]
    public partial struct GoBattleGridSystem : ISystem
    {
        //[BurstCompile]
        //public void OnCreate(ref SystemState state)
        //{

        //}

        //[BurstCompile]
        //public void OnDestroy(ref SystemState state)
        //{

        //}

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var item in SystemAPI.Query<RefRW<MaterialAlphaProperty>>().WithAll<GoBattleEvent>())
            {
                item.ValueRW.value = 0;
            }
        }
    }
}

