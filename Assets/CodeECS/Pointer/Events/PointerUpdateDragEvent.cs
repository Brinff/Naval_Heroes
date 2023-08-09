using Unity.Entities;

namespace Game.Pointer.Events
{
    public struct PointerUpdateDragEvent : IComponentData, IEnableableComponent
    {
        public int value;
    }
}

