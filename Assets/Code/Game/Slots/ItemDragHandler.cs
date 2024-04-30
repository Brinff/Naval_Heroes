using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Code.Game.Slots.DragAndDrop;

namespace Code.Game.Slots
{
    [System.Serializable]
    public class ItemDragHandler : DragAndDrop.IDragAndDropHandler
    {
        [SerializeField] private Item m_Item;
        public Item item => m_Item;
        private PointerEventData m_EventData;
        [SerializeField] private Slot m_SourceSlot;
        public Slot sourceSlot => m_SourceSlot;
        public Slot destinationSlot => m_DestinationSlot;
        
        [SerializeField] private Slot m_DestinationSlot;
        [SerializeField] private Camera m_Camera;
        public Vector3 position { get; private set; }

        private List<IDropTarget> m_DropAreas;

        public ItemDragHandler(Item item, List<IDropTarget> dropAreas)
        {
            m_Item = item;
            m_DropAreas = dropAreas;
        }

        public ItemDragHandler SetCamera(Camera camera)
        {
            m_Camera = camera;
            return this;
        }

        public ItemDragHandler SetEventData(PointerEventData eventData)
        {
            m_EventData = eventData;
            return this;
        }

        public void OnBeginDrag()
        {
        }

        public void OnEndDrag()
        {
            m_EventData = null;

            if (Validate()) Apply();
            else Discard();

            m_CurrentDropTarget.As<IExitDragHandler>()?.OnExitDragHandler(this);
        }

        public delegate void ApplyDelegate(ItemDragHandler itemDragHandler);

        public delegate void DiscardDelegate(ItemDragHandler itemDragHandler);

        public delegate bool ValidateApplyDelegate(ItemDragHandler itemDragHandler);

        public event ApplyDelegate OnApply;
        public event ValidateApplyDelegate OnValidate;
        public event DiscardDelegate OnDiscard;


        public bool Validate()
        {
            var isValid = true;
            if (OnValidate != null)
            {
                foreach (ValidateApplyDelegate func in OnValidate.GetInvocationList())
                {
                    isValid &= func.Invoke(this);
                }
            }

            return m_DestinationSlot != null && isValid;
        }

        public void Apply()
        {
            if (Validate())
            {
                OnApply?.Invoke(this);
                OnApply = null;
                OnValidate = null;
            }
        }

        public ItemDragHandler SetSource(Slot slot)
        {
            m_SourceSlot = slot;
            return this;
        }

        public ItemDragHandler SetDestination(Slot slot)
        {
            m_DestinationSlot = slot;
            return this;
        }

        public void Discard()
        {
            m_Item.transform.position = m_SourceSlot.socket.transform.position;
            OnDiscard?.Invoke(this);
            OnDiscard = null;
        }

        private IDropTarget m_CurrentDropTarget;

        public IDropTarget GetDropArea()
        {
            List<(IDropTarget area, float weight)> areas = new List<(IDropTarget area, float weight)>();

            for (int i = 0; i < m_DropAreas.Count; i++)
            {
                var dropArea = m_DropAreas[i];
                if (dropArea.Overlap(this, out float weight))
                {
                    areas.Add(new ValueTuple<IDropTarget, float>(dropArea, weight));
                }
            }

            if (areas.Count > 0)
            {
                var firstArea = areas.OrderByDescending(x => x.weight).First();
                return firstArea.area;
            }

            return null;
        }

        public void OnDrag()
        {
            if (m_EventData != null && m_Camera)
            {
                var ray = m_Camera.ScreenPointToRay(m_EventData.position);
                Plane plane = new Plane(Vector3.up, 0);
                if (plane.Raycast(ray, out float distance))
                {
                    position = ray.GetPoint(distance);

                    var overlapDropArea = GetDropArea();

                    if (m_CurrentDropTarget != overlapDropArea)
                    {
                        m_DestinationSlot = null;
                        m_CurrentDropTarget.As<IExitDragHandler>()?.OnExitDragHandler(this);
                        m_CurrentDropTarget = overlapDropArea;
                        m_CurrentDropTarget.As<IEnterDragHandler>()?.OnEnterDragHandler(this);
                    }

                    m_CurrentDropTarget.As<IStayDragHandler>()?.OnStayDragHandler(this);

                    m_Item.transform.position = position;
                }
            }
        }
    }
}