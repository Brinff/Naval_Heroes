using UnityEngine;
using UnityEngine.EventSystems;

using IPointerBeginDragHandler = UnityEngine.EventSystems.IBeginDragHandler;
using IPointerDragHandler = UnityEngine.EventSystems.IDragHandler;
using IPointerEndDragHandler = UnityEngine.EventSystems.IEndDragHandler;

namespace Code.Game.Slots
{
    public class Slot : MonoBehaviour, IPointerClickHandler, IPointerBeginDragHandler, IPointerDragHandler, IPointerEndDragHandler
    {
        [SerializeField] private RectTransform m_Socket;
        [SerializeField] private Area m_Area;
        public Area area => m_Area;
        public RectTransform socket => m_Socket;
        private Item m_Item;
        public Item item => m_Item;
        
        public void Put(Item item)
        {
            m_Item = item;
            item.transform.position = m_Socket.position;
            item.transform.SetParent(m_Socket);
        }

        public void Clear()
        {
            m_Item = null;
        }

        private ISlotListener m_SlotListener;

        public Slot SetSlotListener(ISlotListener slotListener)
        {
            m_SlotListener = slotListener;
            return this;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_SlotListener.As<ISlotClickHandler>()?.OnSlotClick(this, eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_SlotListener.As<ISlotBeginDragHandler>()?.OnSlotBeginDrag(this, eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_SlotListener.As<ISlotDragHandler>()?.OnSlotDrag(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_SlotListener.As<ISlotEndDragHandler>()?.OnSlotEndDrag(this, eventData);
        }
    }
}