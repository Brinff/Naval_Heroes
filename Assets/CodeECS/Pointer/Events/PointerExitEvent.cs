using Unity.Entities;

namespace Game.Pointer.Events
{
    public struct PointerExitEvent : IComponentData, IEnableableComponent
    {
        public int value;
    }
}

