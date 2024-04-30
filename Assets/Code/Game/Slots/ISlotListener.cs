using UnityEngine.EventSystems;

namespace Code.Game.Slots
{
    public interface ISlotListener
    {
        
    }
    
    public interface ISlotBeginDragHandler : ISlotListener
    {
        void OnSlotBeginDrag(Slot slot, PointerEventData eventData);
    }
    public interface ISlotDragHandler : ISlotListener
    {
        void OnSlotDrag(Slot slot, PointerEventData eventData);
    }
    
    public interface ISlotEndDragHandler : ISlotListener
    {
        void OnSlotEndDrag(Slot slot, PointerEventData eventData);
    }
    
    public interface ISlotClickHandler : ISlotListener
    {
        void OnSlotClick(Slot slot, PointerEventData eventData);
    }
}