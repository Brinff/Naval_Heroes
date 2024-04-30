using Code.Game.Slots.DragAndDrop;
using Code.Services;
using Game.Merge.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Game.Slots.Merge
{
    public class MergeMediator : MonoBehaviour, IService, IInitializable, ISlotBeginDragHandler, ISlotEndDragHandler,
        IEnterDragHandler,
        IStayDragHandler,
        IExitDragHandler
    {
        private MergeView m_MergeView;
        private DragAndDropService m_DragAndDropService;
        private MergeService m_MergeService;
        private ItemFactory m_ItemFactory;

        private void OnEnable()
        {
            ServiceLocator.Register(this);
        }

        private void OnDisable()
        {
            ServiceLocator.Unregister(this);
        }

        public void Initialize()
        {
            m_MergeView = ServiceLocator.Get<MergeView>();
            m_MergeService = ServiceLocator.Get<MergeService>();
            m_ItemFactory = ServiceLocator.Get<ItemFactory>();
            
            m_DragAndDropService = ServiceLocator.Get<DragAndDropService>();
            m_DragAndDropService.RegisterDropTarget(this);

            foreach (var slot in m_MergeView.slotGroup.slots)
            {
                slot.SetSlotListener(this);
            }
        }

        public bool Overlap(IDragAndDropHandler dragHandler, out float weight)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null)
            {
                weight = 0;
                return false;
            }

            return m_MergeView.area.Overlap(itemDragHandler.position, new Rect(0, 0, 10, 10), out weight);
        }

        public void OnStayDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;

            var overlapSlot = m_MergeView.slotGroup.GetSlotOverlap(itemDragHandler.position, new Rect(0, 0, 10, 10));
            itemDragHandler.SetDestination(overlapSlot);
            Debug.Log("OnStayDragHandler");
        }

        public void OnExitDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;
            itemDragHandler.OnValidate -= Validate;
            itemDragHandler.OnApply -= ApplyDestination;
            Debug.Log("OnExitDragHandler");
        }

        private void ApplyDestination(ItemDragHandler itemDragHandler)
        {
            if(itemDragHandler.destinationSlot.item != null)
            {
                var mergeData = m_MergeService.Merge(itemDragHandler.destinationSlot.item.entityData,
                    itemDragHandler.item.entityData);
                Merge(itemDragHandler.destinationSlot, itemDragHandler.destinationSlot.item, itemDragHandler.item, mergeData);
                Debug.Log(mergeData);
            }
            else itemDragHandler.destinationSlot.Put(itemDragHandler.item);
        }

        private void ApplySource(ItemDragHandler itemDragHandler)
        {
            itemDragHandler.sourceSlot.Clear();
        }

        public void Merge(Slot targetSlot, Item a, Item b, EntityData result)
        {
           var newItem = m_ItemFactory.Create(result);
           m_ItemFactory.Destroy(a);
           m_ItemFactory.Destroy(b);
           targetSlot.Put(newItem);
        }

        private bool Validate(ItemDragHandler itemDragHandler)
        {
            Debug.Log("Validate");
            if (itemDragHandler.destinationSlot != null && itemDragHandler.destinationSlot.item != null)
            {
                return m_MergeService.IsPossibleMerge(itemDragHandler.destinationSlot.item.entityData,
                    itemDragHandler.item.entityData);
            }
            return itemDragHandler.destinationSlot.item == null;
        }

        public void OnEnterDragHandler(IDragAndDropHandler dragHandler)
        {
            var itemDragHandler = dragHandler as ItemDragHandler;
            if (itemDragHandler == null) return;
            itemDragHandler.OnValidate += Validate;
            itemDragHandler.OnApply += ApplyDestination;
            Debug.Log("OnEnterDragHandler");
        }

        private ItemDragHandler m_ItemDragHandler;
        public void OnSlotBeginDrag(Slot slot, PointerEventData eventData)
        {
            if (m_ItemDragHandler != null || slot.item == null) return;

            m_ItemDragHandler = m_DragAndDropService.Create(slot.item)
                .SetSource(slot)
                .SetCamera(Camera.main)
                .SetEventData(eventData);

            m_ItemDragHandler.OnApply += ApplySource;
            m_ItemDragHandler.OnBeginDrag();
        }

        public void OnSlotEndDrag(Slot slot, PointerEventData eventData)
        {
            if (m_ItemDragHandler == null) return;
            m_ItemDragHandler.OnEndDrag();
            m_DragAndDropService.Dispose(m_ItemDragHandler);
            m_ItemDragHandler = null;
        }
    }
}