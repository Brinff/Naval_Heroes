using Unity.Entities;

namespace Game.Pointer.Events
{
    public struct PointerEnterEvent : IComponentData, IEnableableComponent
    {
        public int value;
    }
}

